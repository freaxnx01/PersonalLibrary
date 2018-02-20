using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace PersonalLibrary.Data
{
    public class Database
    {
        public static DataSet ExecuteQuery(string provider /* z.B. Oracle.DataAccess.Client */, string connectionString, string sqlCommand)
        {
            DataSet ds = new DataSet();

            var providerFactory = DbProviderFactories.GetFactory(provider);

            using (var connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                var command = providerFactory.CreateCommand();
                command.CommandText = sqlCommand;
                command.Connection = connection;

                var dataAdapter = providerFactory.CreateDataAdapter();
                dataAdapter.SelectCommand = command;

                dataAdapter.Fill(ds);

                connection.Close();
            }

            return ds;
        }
    }
}
