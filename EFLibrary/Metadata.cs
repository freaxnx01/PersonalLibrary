using System;
using System.Linq;
using System.Xml;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Metadata.Edm;
using System.Data.Common;
using System.Reflection;

using SqlLibrary;
using PersonalLibrary.Data;
using System.IO;
using System.Text;

namespace EFLibrary
{
    public class Metadata
    {
        private ObjectContext objectContext;
        private DatabaseProviders databaseProviders;

        public Metadata(DbContext dbContext, DatabaseProviders databaseProviders)
        {
            objectContext = dbContext.GetObjectContext();
            objectContext.MetadataWorkspace.LoadFromAssembly(Reflection.GetModelAssembly(dbContext));
            this.databaseProviders = databaseProviders;
        }

        public Dictionary<string, string> GetAttributeToColumnMapping(DbContext dbContext, Type entityType)
        {
            var objectContext = dbContext.GetObjectContext();
            var attributeNames = objectContext.MetadataWorkspace.GetItem<EntityType>(entityType.FullName, DataSpace.CSpace).Properties;
            var columnNames = objectContext.MetadataWorkspace.GetItem<EntityType>(dbContext.GetStoreNamespace() + "." + entityType.Name, DataSpace.SSpace).Properties;

            Dictionary<string, string> mappingDictionary = new Dictionary<string, string>();

            for (int i = 0; i < attributeNames.Count; i++)
            {
                mappingDictionary.Add(attributeNames[i].Name, columnNames[i].Name);
            }

            return mappingDictionary;
        }

        //public EntityType GetEntityTypeByModelClass(Type modelClassType)
        //{
        //    var setName = GetContainer().BaseEntitySets.First(s => s.ElementType.Name == modelClassType.Name).Name;
        //    return objectContext.MetadataWorkspace.GetItem<EntityType>(string.Concat(GetNamespaceName(), ".", modelClassType.Name), DataSpace.CSpace);
        //}

        public string GetTableName(DbContext dbContext, Type entityType)
        {
            string entitySQL = string.Format("SELECT VALUE a FROM {0}.{1} AS a", dbContext.GetContainer().Name, dbContext.GetEntitySetName(entityType));
            string traceString = dbContext.GetObjectContext().CreateQuery<DbDataRecord>(entitySQL, new ObjectParameter[0]).ToTraceString();
            return SqlParser.ExtractTableName(traceString);
        }

        public string GetColumnName(DbContext dbContext, PropertyInfo propertyInfo)
        {
            Type modelClassType = propertyInfo.DeclaringType;
            string entitySQL = string.Format("SELECT a.{0} FROM {1}.{2} AS a", propertyInfo.Name, dbContext.GetContainer().Name, dbContext.GetEntitySetName(modelClassType));
            string traceString = objectContext.CreateQuery<DbDataRecord>(entitySQL, new ObjectParameter[0]).ToTraceString();
            return SqlParser.ExtractColumnName(traceString);
        }

        private SqlParser SqlParser
        {
            get
            {
                return new SqlParser(databaseProviders);
            }
        }

        public static MetadataWorkspace FromEdmx(byte[] edmx)
        {
            return FromEdmx(GetTextFromByteArray(edmx));
        }

        private static string GetTextFromByteArray(byte[] byteArray)
        {
            string tempFile = Path.GetTempFileName();
            File.WriteAllBytes(tempFile, byteArray);
            string text = File.ReadAllText(tempFile, Encoding.UTF8);
            File.Delete(tempFile);
            return text;
        }

        public static MetadataWorkspace FromEdmx(string edmx)
        {
            string[] files = EdmxHelper.SplitEdmxToFiles(edmx);
            MetadataWorkspace mdws = new MetadataWorkspace(files, new Assembly[] { });
            files.ToList().ForEach(f => File.Delete(f));
            return mdws;
        }
    }
}
