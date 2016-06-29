using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CCWin;
using System.Net;
using System.Net.Sockets;

namespace testClient
{
    public partial class FormRegister : CCSkinMain
    {
        TcpClient myclient;
        NetworkStream networkStream;
        byte[] buffer; //缓存接收到和要发送的信息
        int size = 8192;

        public FormRegister()
        {
            InitializeComponent();
        }
        
        private void FormRegister_Load(object sender, EventArgs e)
        {
            /*与服务器连接*/
            IPAddress Ip = IPAddress.Parse("127.0.0.1");
            int Port = 2333;
            IPEndPoint EndPoint = new IPEndPoint(Ip, Port);
            myclient = new TcpClient();
            myclient.Connect(EndPoint);

            /*获得网络流*/
            networkStream = myclient.GetStream();
            buffer = new byte[size];

        }

        /*
         [register]
         <Register>
            <Info name="Shane" pwd="1111" header = "1" gender="男" sign="西城主唱" age="21" constellation="12" blood="1"></Info>
         </Register> 
         */


        private void btn_select_Click(object sender, EventArgs e)
        {

        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            myInfo.MsgReadyToBeSentToServer = "[register]<Register><Info name=\"Shane\" pwd=\"1111\" header=\"1\" gender=\"男\" sign=\"西城主唱\" age=\"21\" constellation=\"12\" blood=\"1\"></Info></Register> ";
            
            string temp = myInfo.MsgReadyToBeSentToServer;//赋给临时变量
            myInfo.MsgReadyToBeSentToServer = string.Empty;//清空，方便再次有消息传过来
            SendMsg(temp);

            //MessageBox.Show("myInfo: " + temp);
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {

        }

        public void SendMsg(string msg)
        {
            msg += "[e]"; //末尾设置分隔符

            byte[] bytes = Encoding.Unicode.GetBytes(msg);
            /*写入网络流，并刷新*/
            try
            {
                networkStream.Write(bytes, 0, bytes.Length);
                networkStream.Flush();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


    }
}
