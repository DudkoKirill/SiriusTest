using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SiriusSqlTest1
{
    class DBUtils
    {
        public static SqlConnection GetDBConnection(string connString)
        {
            SqlConnection conn = new SqlConnection(connString);

            return conn;
        }
    }
}
