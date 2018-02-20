using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;

namespace SqlServerCeLibrary
{
    public class Data
    {
        public static DataTable ExecuteSqlAndGetDataTable(string connectionString, string sqlCommand)
        {
            SqlCeConnection conn = new SqlCeConnection(connectionString);
            conn.Open();

            SqlCeCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = sqlCommand;

            SqlCeDataAdapter adapter = new SqlCeDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            conn.Close();

            return dt;
        }

        public static int ReturnLastInsertedIdentityValue(string connectionString)
        {
            int identityValue = 0;

            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                conn.Open();
                identityValue = ReturnLastInsertedIdentityValue(conn);
                conn.Close();
            }

            return identityValue;
        }

        public static int ReturnLastInsertedIdentityValue(SqlCeConnection connection)
        {
            SqlCeCommand cmdIdentity = connection.CreateCommand();
            cmdIdentity.CommandType = System.Data.CommandType.Text;
            cmdIdentity.CommandText = "SELECT @@IDENTITY";
            return int.Parse(cmdIdentity.ExecuteScalar().ToString());
        }
    }
}
