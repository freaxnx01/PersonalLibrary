using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SqlServerCeLibrary
{
    public class Schema
    {
        private DataTable schemaTables;
        private DataTable schemaColumns;

        public Schema(string connectionString)
        {
            schemaTables = Data.ExecuteSqlAndGetDataTable(connectionString, "SELECT * FROM INFORMATION_SCHEMA.TABLES");
            schemaColumns = Data.ExecuteSqlAndGetDataTable(connectionString, "SELECT * FROM INFORMATION_SCHEMA.COLUMNS");
        }

        public bool IsAutoIncrementColumn(string tableName, string columnName)
        {
            var isAutoIncrement = (from c in schemaColumns.AsEnumerable()
                                   where c.Field<string>("TABLE_NAME") == tableName && c.Field<string>("COLUMN_NAME") == columnName
                                   select c.Field<object>("AUTOINC_INCREMENT")).First();
            return isAutoIncrement != null;
        }
    }
}
