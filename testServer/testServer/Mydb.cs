using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace testServer
{
    public class mydb
    {
        private string scn;
        private SqlConnection cn;
        private SqlDataAdapter da;
        private SqlCommand cm;
        public mydb()
        {
            scn = "Data Source=.;Initial Catalog=MyChat;User Id=sa;Password=235721";
            cn = new SqlConnection(scn);
        }

        public bool Sqltest()
        {
            scn = "Data Source=.;Initial Catalog=MyChat;User Id=sa;Password=235721";
            string sql = "select top 1 * from Users";
            try
            {
                cm = new SqlCommand(sql, cn);
                cn.Open();
                if (cm.ExecuteScalar().ToString().Trim() != string.Empty)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                cn.Close();
            }
            return false;
        }

        public int QueryLogin(string sql)
        {
            int i = 1;
            cm = new SqlCommand(sql, cn);
            cn.Open();
            if (cm.ExecuteScalar().ToString() == "0")
                i = -1;
            cn.Close();
            return i;
        }

        public DataSet Query(string sql)
        {
            DataSet ds = new DataSet();
            da = new SqlDataAdapter(sql, cn);
            da.Fill(ds, "Users");
            cn.Close();
            return ds;
        }

        public int QueryReturn(string sql)
        {
            cm = new SqlCommand(sql, cn);
            cn.Open();
            int i = 0;
            try
            {
                i = cm.ExecuteNonQuery();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }

            cn.Close();
            return i;
        }

        public void QueryNoReturn(string sql)
        {
            cm = new SqlCommand(sql, cn);
            cn.Open();
            cm.ExecuteNonQuery();
            cn.Close();
        }

    }
}
