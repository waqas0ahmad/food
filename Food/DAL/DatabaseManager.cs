using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Food.DAL
{
    public class DatabaseManager
    {
        SqlConnection con = null;
        SqlDataAdapter sda = null;
        SqlDataReader sdr = null;
        SqlCommand cmd = null;
        string conStr = "";
        public DatabaseManager()
        {
            conStr = ConfigurationManager.ConnectionStrings["Food_Con"].ConnectionString;
            con = new SqlConnection(conStr);
        }
        void OpenConnection()
        {
            if (con != null)
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
            }
        }
        void CloseConnection()
        {
            if (con != null)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        public DataTable Select(string q)
        {
            try
            {
                DataTable dt = new DataTable();
                OpenConnection();
                cmd = new SqlCommand(q, con);
                sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                CloseConnection();
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int InsertUpdateDelete(string q)
        {
            try
            {
                int result = -1;
                OpenConnection();
                cmd = new SqlCommand(q, con);
                result = cmd.ExecuteNonQuery();
                CloseConnection();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}