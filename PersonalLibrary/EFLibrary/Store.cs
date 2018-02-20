using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Reflection;

using Extensions;
using SqlLibrary;
using PersonalLibrary.Data;

namespace EFLibrary
{
    public class Store
    {
        private DatabaseProviders databaseProvider;

        public Store(DatabaseProviders databaseProvider)
        {
            this.databaseProvider = databaseProvider;
        }

        private SqlBuilder SqlBuilder
        {
            get
            {
                return new SqlBuilder(databaseProvider);
            }
        }

        public string GetCreateIndexSqlStatements(DbContext context)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Type modelType in Reflection.GetDbSetPropertyGenericArgumentTypes(context))
            {
                string sqlCommand = GetCreateIndexSqlStatement(context, modelType);
                if (!sqlCommand.IsNullOrEmpty())
                {
                    sb.AppendLine(sqlCommand);
                }
            }

            return sb.ToString().Trim();
        }

        public string GetCreateIndexSqlStatement(DbContext context, Type modelType)
        {
            List<PropertyInfo> propertyInfos = (from p in modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                                where p.GetCustomAttributes(typeof(StoreIndexAttribute), false).Length > 0
                                                select p).ToList();

            string sqlCommand = string.Empty;

            if (propertyInfos.Count > 0)
            {
                Metadata metadata = new Metadata(context, databaseProvider);
                string tableName = metadata.GetTableName(context, modelType);

                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    string columnName = metadata.GetColumnName(context, propertyInfo);
                    StoreIndexAttribute attribute = propertyInfo.GetCustomAttributes(typeof(StoreIndexAttribute), false)[0] as StoreIndexAttribute;

                    if (attribute.Unique)
                    {
                        sqlCommand = SqlBuilder.CreateUniqueIndex(tableName, columnName);
                    }
                    else
                    {
                        sqlCommand = SqlBuilder.CreateIndex(tableName, columnName);
                    }
                }
            }

            return sqlCommand;
        }

        public void CreateIndexes(DbContext context)
        {
            foreach (Type modelType in Reflection.GetDbSetPropertyGenericArgumentTypes(context))
            {
                string sqlCommand = GetCreateIndexSqlStatement(context, modelType);
                if (!sqlCommand.IsNullOrEmpty())
                {
                    context.Database.ExecuteSqlCommand(sqlCommand);
                }
            }
        }

        public string GetDdlScript(DbContext dbContext)
        {
            return dbContext.GetObjectContext().CreateDatabaseScript();
        }
    }
}
