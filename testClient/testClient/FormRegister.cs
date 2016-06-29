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
            ////IPAddress Ip = IPAddress.Parse("127.0.0.1");
            //IPAddress Ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[2];
            //int Port = 12150;
            IPEndPoint EndPoint = new IPEndPoint(IpConfig.getIpv4(), IpConfig.Port);

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
            //myInfo.MsgReadyToBeSentToServer = "[register]<Register><Info name=\"Shane\" pwd=\"1111\" header=\"1\" gender=\"男\" sign=\"西城主唱\" age=\"21\" constellation=\"12\" blood=\"1\"></Info></Register> ";

            int header = 0;
            string gender = string.Empty;
            string sign = string.Empty;
            int constellation = 0;
            int blood = 0;

            try
            {
                if (rbtn_male.Checked == true)
                    gender = "男";
                else if (rbtn_female.Checked == true)
                    gender = "女";

                header = 5;
                sign = tb_sign.Text;
                constellation = cb_con.SelectedIndex + 1; //1~12
                blood = cb_blood.SelectedIndex + 1; //1~4
                //MessageBox.Show(constellation + " " + blood);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            if (tb_name.Text == string.Empty)
            {
                MessageBox.Show("名称不能为空");
            }
            else if (tb_pwd.Text == string.Empty)
            {
                MessageBox.Show("密码不能为空");
            }
            else if (tb_pwd_confirm.Text != tb_pwd.Text)
            {
                MessageBox.Show("前后两次密码不一致");
            }
            else if (gender == string.Empty)
            {
                MessageBox.Show("性别不能为空");
            }
            else if (tb_age.Text == string.Empty)
            {
                MessageBox.Show("年龄不能为空");
            }
            else
            {
                string reg = string.Format("[register]<Register><Info name=\"{0}\"  pwd=\"{1}\" header=\"{2}\" gender=\"{3}\" sign=\"{4}\" "
                + "age=\"{5}\" constellation=\"{6}\" blood=\"{7}\"></Info></Register> ", tb_name.Text, tb_pwd.Text, header,
                gender, sign, tb_age.Text, constellation, blood);

                //myInfo.MsgReadyToBeSentToServer = "[register]<Register><Info name=\"Shane\" pwd=\"1111\" header=\"1\" gender=\"男\" sign=\"西城主唱\" age=\"21\" constellation=\"12\" blood=\"1\"></Info></Register> ";
                myInfo.MsgReadyToBeSentToServer = reg;

                string temp = myInfo.MsgReadyToBeSentToServer;//赋给临时变量
                myInfo.MsgReadyToBeSentToServer = string.Empty;//清空，方便再次有消息传过来
                SendMsg(temp);

                lock (networkStream)
                {
                    AsyncCallback asy = new AsyncCallback(ReceiveMsg);
                    networkStream.BeginRead(buffer, 0, size, asy, null);
                }
            }

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


        public void ReceiveMsg(IAsyncResult result)
        {
            try
            {
                int readbytes;
                /*结束异步的BeginRead，获取所读字节数*/
                lock (networkStream)
                {
                    readbytes = networkStream.EndRead(result);
                }
                if (readbytes <= 0) //若出现传输字节为0时不做处理
                {
                    //
                }
                else //正常接收信息
                {

                    //将字节数组buffer解码为字符串，并清空buffer以便接收下一条由服务器传来的信息
                    string Readmsg = Encoding.Unicode.GetString(buffer, 0, readbytes);
                    Array.Clear(buffer, 0, buffer.Length);//清空缓存

                    //MessageBox.Show(Readmsg);

                    if (Readmsg.StartsWith("[Account]"))
                    {
                        MessageBox.Show("注册成功，您的账号是：" + Readmsg.Substring(9));
                    }
                }
                lock (networkStream)
                {
                    AsyncCallback asy = new AsyncCallback(ReceiveMsg);
                    networkStream.BeginRead(buffer, 0, size, asy, null);
                }
            }
            catch (Exception ex)
            {
                //出现异常用户断开
                MessageBox.Show("breakdown: " + ex.Message);
            }
        }


    }
}
