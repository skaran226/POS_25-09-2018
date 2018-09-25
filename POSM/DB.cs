using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSM
{
    class DB
    {
        static SqlConnection Conn = new SqlConnection(Macros.LOCAL_DB_CONN);

        public static SqlConnection OpenConn()
        {
            if (Conn.State == System.Data.ConnectionState.Closed)
            {
                Conn.Open();
            }
            return Conn;
        }

        public static void CloseConn()
        {
            if (Conn.State == System.Data.ConnectionState.Open)
            {
                Conn.Close();
            }
        }

        public static void ExecuteNonQuery(string sPassQuery)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(sPassQuery, OpenConn());
                //cmd.CommandType = System.Data.CommandType.Text;
                
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //ErrorLogEvents(ex.ToString());
            }
            //CloseConn();
        }

        public static SqlCommand ExecuteReader(string sPassQuery)
        {
            SqlCommand cmd = null;
            try
            {
               cmd = new SqlCommand(sPassQuery, OpenConn());

                //cmd.CommandType = System.Data.CommandType.Text;

                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //ErrorLogEvents(ex.ToString());
            }
            return cmd;
            //CloseConn();
        }
    }
}
