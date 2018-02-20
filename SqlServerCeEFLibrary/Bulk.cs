using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Collections;
using System.Data.SqlServerCe;
using System.Reflection;

using EFLibrary;
using SqlServerCeLibrary;
using LinqToEdmx;
using LinqToEdmx.Model.Conceptual;
using LinqToEdmx.Map;
using System.Diagnostics;

namespace SqlServerCeEFLibrary
{
    public class Bulk
    {
        private SqlCeConnection connection;
        private Schema schemaLib;
        private Edmx edmx;
        private Dictionary<Type, PropertyInfo> entityKeyPropertyCache = new Dictionary<Type, PropertyInfo>();
        private Dictionary<Tuple<string, string>, Tuple<PropertyInfo, string>> parentChildRelationCache = new Dictionary<Tuple<string,string>,Tuple<PropertyInfo,string>>();
        private Dictionary<string, Tuple<Dictionary<PropertyInfo, int>, PropertyInfo>> scalarMappingCache = new Dictionary<string, Tuple<Dictionary<PropertyInfo, int>, PropertyInfo>>();
        private Dictionary<string, string> tableNameCache = new Dictionary<string, string>();
        private Dictionary<Tuple<string, string>, Tuple<int, PropertyInfo>> lookupMappingCache = new Dictionary<Tuple<string, string>, Tuple<int, PropertyInfo>>();
        private Dictionary<Type, List<NavigationProperty>> listPropertiesCache = new Dictionary<Type, List<NavigationProperty>>();
        private Dictionary<Type, Dictionary<PropertyInfo, int>> ordinalLookupMappingCache = new Dictionary<Type, Dictionary<PropertyInfo, int>>();

        SqlCeCommand commandIdentity;

        public void BulkInsert(DbContext dbContext, object objectToInsert)
        {
            DoBulkInsert(dbContext, new object[] { objectToInsert });
        }

        public void BulkInsert(DbContext dbContext, IEnumerable<object> objectsToInsert)
        {
            DoBulkInsert(dbContext, objectsToInsert);
        }

        private void DoBulkInsert(DbContext dbContext, IEnumerable<object> objectsToInsert)
        {
            string connectionString = dbContext.Database.Connection.ConnectionString;

            edmx = dbContext.GetEdmxHelper();
            schemaLib = new Schema(connectionString);

            connection = new SqlCeConnection(connectionString);

            commandIdentity = connection.CreateCommand();
            commandIdentity.CommandType = System.Data.CommandType.Text;
            commandIdentity.CommandText = "SELECT @@IDENTITY";
            
            connection.Open();
            DoBulkInsert(dbContext, null, string.Empty, objectsToInsert);
            connection.Close();
        }

        private string GetTableName(string entityName)
        {
            if (tableNameCache.ContainsKey(entityName))
            {
                return tableNameCache[entityName];
            }

            string tableName = edmx.GetTableName(entityName);
            tableNameCache.Add(entityName, tableName);
            return tableName;
        }

        private void DoBulkInsert(DbContext dbContext, object parent, string childPropertyName, IEnumerable<object> objectsToInsert)
        {
            Type entityClrType = objectsToInsert.First().GetType();
            string entityName = entityClrType.Name;

            //Debug.Print(entityName);
            
            PropertyInfo entityKeyPropertyInfo = GetEntityKeyPropertyInfo(entityClrType);

            SqlCeCommand cmd = connection.CreateCommand();
            cmd.CommandType = System.Data.CommandType.TableDirect;
            cmd.CommandText = GetTableName(entityName);

            SqlCeResultSet rs = cmd.ExecuteResultSet(ResultSetOptions.Updatable | ResultSetOptions.Scrollable);

            // Scalar Properties
            // AuotIncrement Property
            PropertyInfo auotIncrementPropInfo = null;
            Dictionary<PropertyInfo, int> ordinalMapping = null;
            if (scalarMappingCache.ContainsKey(entityName))
            {
                ordinalMapping = scalarMappingCache[entityName].Item1;
                auotIncrementPropInfo = scalarMappingCache[entityName].Item2;
            }
            else
            {
                Dictionary<string, string> scalarMappingDictionary = edmx.GetScalarPropertiesMappingDictionary(entityName);
                Dictionary<string, string> revisedColumnMapping = RemoveAutoIncrementColumns(GetTableName(entityName), scalarMappingDictionary);
                ordinalMapping = GetAttributeToColumnOrdinalMapping(dbContext, entityClrType, rs, revisedColumnMapping);

                string auotIncrementPropertyName = scalarMappingDictionary.Keys.Except(revisedColumnMapping.Keys).SingleOrDefault();
                if (!string.IsNullOrEmpty(auotIncrementPropertyName))
                {
                    auotIncrementPropInfo = entityClrType.GetProperty(auotIncrementPropertyName);
                }

                scalarMappingCache.Add(entityName, Tuple.Create(ordinalMapping, auotIncrementPropInfo));
            }

            // Navigation Properties, Lookups
            if (!ordinalLookupMappingCache.ContainsKey(entityClrType))
            {
                ordinalLookupMappingCache.Add(entityClrType,
                    GetAttributeToColumnOrdinalMapping(dbContext, entityClrType, rs, edmx.GetLookupPropertiesMappingDictionary(entityClrType)));
            }
            Dictionary<PropertyInfo, int> ordinalLookupMapping = ordinalLookupMappingCache[entityClrType];

            // Navigation Properties, Lists
            if (!listPropertiesCache.ContainsKey(entityClrType))
            {
                listPropertiesCache.Add(entityClrType, edmx.GetListProperties(entityClrType));
            }
            List<NavigationProperty> listNavigationProperties = listPropertiesCache[entityClrType];

            // Parent
            PropertyInfo parentKeyPropertyInfo = null;
            int parentColumnOrdinal = 0;
            if (parent != null)
            {
                Type parentType = parent.GetType();
                string parentEntityName = parentType.Name;
                string columnName = string.Empty;

                if (parentChildRelationCache.ContainsKey(Tuple.Create(parentEntityName, childPropertyName)))
                {
                    parentKeyPropertyInfo = parentChildRelationCache[Tuple.Create(parentEntityName, childPropertyName)].Item1;
                    columnName = parentChildRelationCache[Tuple.Create(parentEntityName, childPropertyName)].Item2;
                }
                else
                {
                    EndProperty endProperty = edmx.GetParentChildRelationEndProperty(parentEntityName, childPropertyName);
                    columnName = endProperty.ScalarProperties.Single().ColumnName;
                    string parentEntityKeyPropertyName = edmx.GetEntityKeyPropertyName(parentEntityName);
                    parentKeyPropertyInfo = parentType.GetProperty(parentEntityKeyPropertyName);
                    parentChildRelationCache.Add(Tuple.Create(parentEntityName, childPropertyName), Tuple.Create(parentKeyPropertyInfo, columnName));
                }

                SqlCeUpdatableRecord recParent = rs.CreateRecord();
                parentColumnOrdinal = recParent.GetOrdinal(columnName);
            }

            foreach (var element in objectsToInsert)
            {
                if (IsObjectAlreadyInserted(element, entityKeyPropertyInfo))
                {
                    continue;
                }

                SqlCeUpdatableRecord rec = rs.CreateRecord();

                // Scalar Properties
                foreach (var ordinalMappingEntry in ordinalMapping)
                {
                    object value = ordinalMappingEntry.Key.GetValue(element, null);
                    rec.SetValue(ordinalMappingEntry.Value, value);
                }

                // Navigation Properties, Lookups
                foreach (var ordinalLookupMappingEntry in ordinalLookupMapping)
                {
                    object lookupInstance = ordinalLookupMappingEntry.Key.GetValue(element, null);
                    if (lookupInstance != null)
                    {
                        DoBulkInsert(dbContext, null, string.Empty, new object[] { lookupInstance });

                        Type lookupType = ordinalLookupMappingEntry.Key.PropertyType;
                        string lookupPropertyName = ordinalLookupMappingEntry.Key.Name;
                        var dictKeyTuple = Tuple.Create(entityName, lookupPropertyName);
                        if (!lookupMappingCache.ContainsKey(dictKeyTuple))
                        {
                            string lookupKeyPropertyName = edmx.GetEntityKeyPropertyName(lookupType.Name);
                            lookupMappingCache.Add(dictKeyTuple,
                                Tuple.Create(ordinalLookupMappingEntry.Value, lookupType.GetProperty(lookupKeyPropertyName)));
                        }

                        int lookupOrdinal = lookupMappingCache[dictKeyTuple].Item1;
                        object value = lookupMappingCache[dictKeyTuple].Item2.GetValue(lookupInstance, null);
                        
                        rec.SetValue(lookupOrdinal, value);
                    }
                }

                if (parent != null)
                {
                    rec.SetValue(parentColumnOrdinal, parentKeyPropertyInfo.GetValue(parent, null));
                }

                rs.Insert(rec);

                if (auotIncrementPropInfo != null)
                {
                    auotIncrementPropInfo.SetValue(element, int.Parse(commandIdentity.ExecuteScalar().ToString()), null);
                }

                // Navigation Properties, Lists
                foreach (NavigationProperty navigationProperty in listNavigationProperties)
                {
                    object value = entityClrType.GetProperty(navigationProperty.Name).GetValue(element, null);
                    if (value != null && value is ICollection)
                    {
                        IEnumerable<object> childList = (IEnumerable<object>)value;
                        if (childList.Count() > 0)
                        {
                            DoBulkInsert(dbContext, element, navigationProperty.Name, childList);
                        }
                    }
                }
            }
        }

        private PropertyInfo GetEntityKeyPropertyInfo(Type entityClrType)
        {
            if (entityKeyPropertyCache.ContainsKey(entityClrType))
            {
                return entityKeyPropertyCache[entityClrType];
            }

            string entityKeyPropertyName = edmx.GetEntityKeyPropertyName(entityClrType.Name);
            PropertyInfo propertyInfo = entityClrType.GetProperty(entityKeyPropertyName);
            if (propertyInfo.PropertyType != typeof(int))
            {
                throw new NotSupportedException();
            }
            entityKeyPropertyCache.Add(entityClrType, propertyInfo);
            
            return propertyInfo;
        }

        private bool IsObjectAlreadyInserted(Object element, PropertyInfo propertyInfo)
        {
            return (int)propertyInfo.GetValue(element, null) > 0;
        }

        private Dictionary<string, string> RemoveAutoIncrementColumns(string tableName, Dictionary<string, string> columnMapping)
        {
            Dictionary<string, string> revisedColumnMapping = new Dictionary<string, string>();

            foreach (var mappingEntry in columnMapping)
            {
                if (!schemaLib.IsAutoIncrementColumn(tableName, mappingEntry.Value))
                {
                    revisedColumnMapping.Add(mappingEntry.Key, mappingEntry.Value);
                }
            }
            
            return revisedColumnMapping;
        }

        private Dictionary<PropertyInfo, int> GetAttributeToColumnOrdinalMapping(DbContext dbContext, Type entityType,
            SqlCeResultSet resultSet, Dictionary<string, string> columnMapping)
        {
            var ordinalMapping = new Dictionary<PropertyInfo, int>();
            SqlCeUpdatableRecord rec = resultSet.CreateRecord();
            foreach (var mappingEntry in columnMapping)
            {
                ordinalMapping.Add(entityType.GetProperty(mappingEntry.Key), rec.GetOrdinal(mappingEntry.Value));
            }

            return ordinalMapping;
        }
    }
}
