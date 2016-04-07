using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace testServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static IPAddress Ip=IPAddress .Parse ("127.0.0.1");
        //public static IPAddress Ip = IPAddress.Parse("10.146.93.17");
        public static int Port=500;
        public static IPEndPoint EndPoint = new IPEndPoint(Ip, Port);

        TcpListener myListener = new TcpListener(EndPoint);
        Thread myThread;
        bool isend;

        private void button1_Click(object sender, EventArgs e)
        {
            myListener = new TcpListener(EndPoint);
            myListener.Start();

            isend = false;
            myThread = new Thread(new ThreadStart(StartListen));
            myThread.Start();

            button1.Enabled = false;
            button2.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //myThread = new Thread(new ThreadStart(StartListen));
        }

        public void StartListen()
        {
            //轮询接受客户端请求
            while (true && !isend)
            {
                MyUser client = new MyUser(myListener.AcceptTcpClient());////////////
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            myListener.Stop();
            myThread.Abort();
            button1.Enabled = true;
            button2.Enabled = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (myListener != null)
            {
                button1.PerformClick();
            }
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            mydb test = new mydb();
            if (test.Sqltest())
            {
                MessageBox.Show("数据库连接成功！");
            }
            else
            {
                MessageBox.Show("连接失败！");
            }
        }
    }
}
