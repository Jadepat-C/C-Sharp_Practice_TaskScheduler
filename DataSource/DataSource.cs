using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskScheduler.DataSource
{
    internal class DataSource
    {
        private SqlConnection conn = null;
        public DataSource() { }

        public SqlConnection CreateConnection()
        {
            try
            {

                SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
                sqlConnectionStringBuilder.UserID = "chernsonthiLogin";
                sqlConnectionStringBuilder.Password = "admin";
                sqlConnectionStringBuilder.InitialCatalog = "TaskScheduler";
                sqlConnectionStringBuilder.DataSource = "(local)";
                string connectionString = sqlConnectionStringBuilder.ConnectionString;

                conn = new SqlConnection(connectionString);

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return conn;
        }
    }
}
