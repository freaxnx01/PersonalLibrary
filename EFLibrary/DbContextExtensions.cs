using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Metadata.Edm;
using System.Xml;
using System.Data.Common;
using System.Data.Objects.DataClasses;

namespace EFLibrary
{
    public static class DbContextExtensions
    {
        public static ObjectContext GetObjectContext(this DbContext dbContext)
        {
            return ((IObjectContextAdapter)dbContext).ObjectContext;
        }

        public static LinqToEdmx.Edmx GetEdmxHelper(this DbContext dbContext)
        {
            return EdmxHelper.Parse(dbContext);
        }

        public static string GetStoreNamespace(this DbContext dbContext)
        {
            var names = dbContext.GetObjectContext().MetadataWorkspace.GetItems(DataSpace.SSpace).Where(i => i is EntityType).ToList();
            string storeNamespace = names[0].ToString().Split('.')[0];
            return storeNamespace;
        }

        //private string GetNamespaceName()
        //{
        //    return objectContext.MetadataWorkspace.GetItems<EntityType>(DataSpace.CSpace).First().NamespaceName;
        //}

        public static EntityContainer GetContainer(this DbContext dbContext)
        {
            return dbContext.GetObjectContext().MetadataWorkspace.GetItems<EntityContainer>(DataSpace.CSpace).First();
        }

        public static string GetEntitySetName(this DbContext dbContext, Type entityType)
        {
            return dbContext.GetContainer().BaseEntitySets.First(s => s.ElementType.Name == entityType.Name).Name;
        }

        public static string GetEdmx(this DbContext context)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriter xmlWriter = XmlWriter.Create(sb);
            EdmxWriter.WriteEdmx(context, xmlWriter);
            return sb.ToString();
        }

        public static void WriteEdmx(this DbContext context, string outputFilePath)
        {
            EdmxWriter.WriteEdmx(context, XmlWriter.Create(outputFilePath, new XmlWriterSettings() { Indent = true }));
        }

        public static Dictionary<string, int> GetOrdinalDictionary(this DbContext dbContext, Type entityType)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();

            object newObject = Activator.CreateInstance(entityType);
            dbContext.Set(entityType).Add(newObject);
            ObjectStateEntry ose = dbContext.GetObjectContext().ObjectStateManager.GetObjectStateEntry(newObject);

            foreach (FieldMetadata fm in ose.CurrentValues.DataRecordInfo.FieldMetadata)
            {
                if (fm.FieldType != null)
                {
                    dict.Add(fm.FieldType.Name, fm.Ordinal);
                }
            }

            dbContext.Set(entityType).Remove(newObject);

            return dict;
        }

        public static List<MigrationHistory> GetMigrationHistory(this DbContext dbContext)
        {
            return MigrationHistory.GetMigrationHistory(dbContext);
        }

        public static string GetCreateDatabaseScript(this DbContext dbContext)
        {
            return dbContext.GetObjectContext().CreateDatabaseScript();
        }

        public static MetadataWorkspace GetMetadataWorkspace(this DbContext dbContext)
        {
            return GetObjectContext(dbContext).MetadataWorkspace;
        }

        public static EntityType GetEntityType(this DbContext dbContext, string name)
        {
            return dbContext.GetMetadataWorkspace().GetItems<EntityType>(DataSpace.OSpace).Where(i => i.Name == name).Single();
        }

        public static AssociationType GetAssociationType(this DbContext dbContext, NavigationProperty navigationProperty)
        {
            return dbContext.GetMetadataWorkspace().GetItems<AssociationType>(DataSpace.OSpace).Where(a => a.Name == navigationProperty.RelationshipType.Name).Single();
        }
    }
}
