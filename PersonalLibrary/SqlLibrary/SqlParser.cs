using System;
using System.Text.RegularExpressions;

using PersonalLibrary.Data;

namespace SqlLibrary
{
    public class SqlParser
    {
        private DatabaseProviders databaseProvider;

        public SqlParser(DatabaseProviders databaseProvider)
        {
            this.databaseProvider = databaseProvider;
        }

        public string ExtractTableName(string sqlCommand)
        {
            /* T-SQL
            SELECT 
            [Extent1].[CategoryId] AS [CategoryId], 
            [Extent1].[Code] AS [Code], 
            [Extent1].[ContentContentId] AS [ContentContentId], 
            [Extent1].[CategoryCategoryId] AS [CategoryCategoryId]
            FROM [dbo].[TABCATEGORY] AS [Extent1]
            */

            switch (databaseProvider)
            {
                case DatabaseProviders.OracleDevart:
                    throw new NotImplementedException();
                case DatabaseProviders.OracleBeta:
                    throw new NotImplementedException();
                case DatabaseProviders.SqlServer:
                    return RunRegexAndReturnGroupValue(new Regex(@"\sFROM\s\[(?<schema>[^\]]*)\]\.\[(?<table>[^\]]*)\]"), sqlCommand, "table");
                case DatabaseProviders.SqlServerCE:
                    return RunRegexAndReturnGroupValue(new Regex(@"\sFROM\s\[(?<table>[^\]]*)\]"), sqlCommand, "table");
            }

            return string.Empty;
        }

        public string ExtractColumnName(string sqlCommand)
        {
            /* T-SQL
            SELECT 
            1 AS [C1], 
            [Extent1].[CODECOLUMN] AS [CODECOLUMN]
            FROM [dbo].[TABCATEGORY] AS [Extent1]
            */

            switch (databaseProvider)
            {
                case DatabaseProviders.OracleDevart:
                    throw new NotImplementedException();
                case DatabaseProviders.OracleBeta:
                    throw new NotImplementedException();
                case DatabaseProviders.SqlServer:
                case DatabaseProviders.SqlServerCE:
                    return RunRegexAndReturnGroupValue(new Regex(@"\[[^\]]*\]\.\[(?<name>[^\]]*)\]"), sqlCommand, "name");
                default:
                    break;
            }

            return string.Empty;
        }

        private string RunRegexAndReturnGroupValue(Regex regex, string input, string groupName)
        {
            Match match = regex.Match(input);
            if (match.Success)
            {
                return match.Groups[groupName].Value;
            }

            return string.Empty;
        }
    }
}
