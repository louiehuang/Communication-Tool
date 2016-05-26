using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;

namespace testServer
{
    class DBConn
    {
        OleDbConnection conn = null;//连接数据库的对象
        //下面是构造函数连接数据库
        public DBConn()
        {
            if (conn == null)//判断连接是否为空
            {
                conn = new OleDbConnection();
                //连接数据库的字符串
                conn.ConnectionString = "provider=sqloledb.1;Data Source=.;Initial Catalog=MyChat;User Id=sa;Password=235721";
            }
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();//打开数据库连接
            }
        }

        //下面这个方法是从数据库中查找数据的方法
        public DataSet DBQuery(string sql)
        {
            DataSet ds = new DataSet();//DataSet是表的集合
            try
            {
                connOpen();
                OleDbDataAdapter da = new OleDbDataAdapter(sql, conn);//从数据库中查询
                da.Fill(ds);//将数据填充到DataSet 
                connClose();//关闭连接
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return ds;//返回结果
        }

        //用户登录查询
        public int DB_LoginQuery(string sql)
        {
            int AffectedRows = 1;
            try
            {
                connOpen();
                OleDbCommand Comm = new OleDbCommand(sql, conn);//不执行SQL语句

                if (Comm.ExecuteScalar().ToString() == "0")
                    AffectedRows = 0;

                connClose();//关闭连接
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return AffectedRows;

        }


        public void DBNonReturn(string sql)
        {
            connOpen();
            OleDbCommand Comm = new OleDbCommand(sql, conn);//不执行SQL语句
            Comm.ExecuteNonQuery();
            connClose();
        }


        //下面的方法是对数据库进行更新
        public int DBcmd(string sql)
        {
            int x = -1;
            try
            {
                connOpen();
                OleDbCommand oc = new OleDbCommand();//表示要对数据源执行的SQL语句或存储过程
                oc.CommandText = sql;//设置命令的文本
                oc.CommandType = CommandType.Text;//设置命令的类型
                oc.Connection = conn;//设置命令的连接
                x = oc.ExecuteNonQuery();//执行SQL语句
                connClose();//关闭连接
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return x; //返回一个影响行数
        }


        public void connOpen()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
        }



        //下面的connClose()方法是关闭数据库连接
        public void connClose()
        {
            if (conn.State == ConnectionState.Open)
            {//判断数据库的连接状态，如果状态是打开的话就将它关闭
                conn.Close();
            }
        }
    }
}
