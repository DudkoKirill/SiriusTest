using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SiriusSqlTest1
{
    class DBSQLServerUtils
    {
        public static SqlConnection
                GetDBConnection(string datasource, string database, string username, string password)
        {
          
        
            SqlConnection conn = new SqlConnection();

            return conn;
        }
    }
}
