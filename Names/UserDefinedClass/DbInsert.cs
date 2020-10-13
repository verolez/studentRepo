using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserDefinedClass
{
    public static class DbInsert
    {
        public static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["dbConString"].ConnectionString; }
        }
        public static string Save(string sp, SqlParameter[] sqlParameters)
        {

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sp, con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddRange(sqlParameters);
                cmd.ExecuteNonQuery();
                return cmd.Parameters["@ReturnValue"].Value.ToString();
            }
        }
    }
}
