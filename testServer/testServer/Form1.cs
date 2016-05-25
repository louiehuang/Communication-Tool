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

        public static ManualResetEvent allDone = new ManualResetEvent(false);

        /// <summary>
        /// 开始监听客户端请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        }

        public void StartListen()
        {
            //轮询接受客户端请求
            while (true && !isend)
            {
                allDone.Set();
                ProcessUserRequest client = new ProcessUserRequest(myListener.AcceptTcpClient());
                allDone.WaitOne();
            }
        }

        /// <summary>
        /// 停止监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            myListener.Stop();
            myThread.Abort();
            button1.Enabled = true;
            button2.Enabled = false;
        }

        /// <summary>
        /// 关闭窗体时停止监听，退出应用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (myListener != null)
            {
                button2.PerformClick();
            }
            Application.Exit();
        }

    }
}
