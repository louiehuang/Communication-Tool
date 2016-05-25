using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;

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
            OleDbDataAdapter da = new OleDbDataAdapter(sql, conn);//从数据库中查询
            da.Fill(ds);//将数据填充到DataSet 
            connClose();//关闭连接
            return ds;//返回结果
        }

        //下面的方法是对数据库进行更新
        public int DBcmd(string sql)
        {
            OleDbCommand oc = new OleDbCommand();//表示要对数据源执行的SQL语句或存储过程
            oc.CommandText = sql;//设置命令的文本
            oc.CommandType = CommandType.Text;//设置命令的类型
            oc.Connection = conn;//设置命令的连接
            int x = oc.ExecuteNonQuery();//执行SQL语句
            connClose();//关闭连接
            return x; //返回一个影响行数
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
