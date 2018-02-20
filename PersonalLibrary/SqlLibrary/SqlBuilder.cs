using System;
using System.Reflection;

using PersonalLibrary.Data;

namespace SqlLibrary
{
    public class SqlBuilder
    {
        private DatabaseProviders databaseProvider;

        public SqlBuilder(DatabaseProviders databaseProvider)
        {
            this.databaseProvider = databaseProvider;
        }

        private Type GetResourceClassType()
        {
            Type thisType = this.GetType();
            return this.GetType().Assembly.GetType(string.Concat(thisType.Namespace, ".", thisType.Name, databaseProvider.ToString()), true);
        }

        private string GetResourceString(string propertyName)
        {
            PropertyInfo propertyInfo = GetResourceClassType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Static);
            return propertyInfo.GetValue(null, null).ToString();
        }

        public string CreateIndex(string table, string column)
        {
            // 0 = Index name
            // 1 = Table name
            // 2 = Column name
            string template = GetResourceString(MethodBase.GetCurrentMethod().Name);
            return string.Format(template, GetIndexName(table, column), table, column);
        }

        public string CreateUniqueIndex(string table, string column)
        {
            // 0 = Index name
            // 1 = Table name
            // 2 = Column name
            string template = GetResourceString(MethodBase.GetCurrentMethod().Name);
            return string.Format(template, GetIndexName(table, column), table, column);
        }

        private string GetIndexName(string table, string column)
        {
            return string.Concat(GetResourceString("IndexNamePrefix"), column);
        }
    }
}
