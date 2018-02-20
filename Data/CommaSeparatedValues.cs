using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Data.Odbc;

namespace PersonalLibrary.Data
{
    public class CommaSeparatedValues
    {
        public static DataTable GetDataTableFromCsv(string path)
        {
            return GetDataTableFromCsv(path, ';');
        }

        public static DataTable GetDataTableFromCsv(string path, char delimiter)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException();

            const string schemaIniContent = "[{0}]\r\nFormat=Delimited({1})";
            const string schemaFileName = "schema.ini";

            string directory = Path.GetDirectoryName(path);
            string schemaFullPath = Path.Combine(directory, schemaFileName);
            bool schemaFileExists = File.Exists(schemaFullPath);

            if (delimiter != ',')
            {
                File.WriteAllText(schemaFullPath, string.Format(schemaIniContent, Path.GetFileName(path), delimiter));
            }

            FileInfo fileInfo = new FileInfo(path);
            DataTable dataTable = new DataTable();
            string connectionString = String.Format("Driver={{Microsoft Text Driver (*.txt; *.csv)}};Dbq={0};", fileInfo.DirectoryName);

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                OdbcDataAdapter da = new OdbcDataAdapter(String.Format("select * from [{0}]", fileInfo.Name), connection);
                da.Fill(dataTable);
            }

            if (delimiter != ',' && !schemaFileExists)
            {
                File.Delete(schemaFullPath);
            }

            return dataTable;
        }

        public static void WriteDataTableToCsvFile(DataTable dataTable, string path, char delimiter, bool includeHeader)
        {
            File.WriteAllText(path, ConvertDataTableToCsv(dataTable, delimiter, includeHeader));
        }

        public static string ConvertDataTableToCsv(DataTable dataTable, char delimiter, bool includeHeader)
        {
            StringBuilder sb = new StringBuilder();
            int numberOfColumns = dataTable.Columns.Count;

            if (includeHeader)
            {
                for (int c = 0; c < numberOfColumns; c++)
                {
                    sb.Append(dataTable.Columns[c].ColumnName);

                    if (c < numberOfColumns)
                    {
                        sb.Append(delimiter);
                    }
                }
            }

            foreach (DataRow row in dataTable.Rows)
            {
                for (int c = 0; c < numberOfColumns; c++)
                {
                    string text = row[c].ToString();
                    if (text.Contains(delimiter))
                    {
                        text = "\"" + text + "\"";
                    }

                    sb.Append(text);

                    if (c < numberOfColumns)
                    {
                        sb.Append(delimiter);
                    }
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
