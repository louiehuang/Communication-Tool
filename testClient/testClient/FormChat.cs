using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CCWin;
using CCWin.SkinClass;
using CCWin.SkinControl;

namespace testClient
{
    public delegate void UpdateChat(byte[] msg);
    public partial class FormChat : CCSkinMain
    {
        int imgindex;
        public bool isalve = false;
        string mynum;
        string hisnum;
        //Bitmap bp;
        byte[] b_mes = new byte[500];
        //Image image;
        string str = "";
        byte[] all; ///////////////////???
        string myname; //我的昵称
        string name; //聊天对象的昵称

        public FormChat()
        {
            InitializeComponent();
        }

        public FormChat(string My, string His, string MyName, string Name, int imageindex)
        {
            InitializeComponent();
            this.mynum = My;
            this.hisnum = His;
            this.myname = MyName;
            this.name = Name;
            imgindex = imageindex;
            isalve = true;
            myInfo.ChatForm.Add(hisnum, this);
        }

        private byte[] mes(string s)                  //得到richtextbox2的文字字节数
        {
            byte[] b_mes = Encoding.Unicode.GetBytes(s);
            return b_mes;
        }

        /// <summary>
        /// 通过FormMain中ProcessPersonalMsg调用，由传来的msg消息更新聊天窗口的richTextBox1
        /// </summary>
        /// <param name="msg"></param>
        public void updatemsg(byte[] msg)
        {
            //MessageBox.Show("in updatemsg");
            byte[] text = new byte[msg.Length];
            for (int i = 0; i < msg.Length; i++)
                text[i] = msg[i];

            richTextBox1.AppendText(Encoding.Unicode.GetString(text, 0, text.Length) + Environment.NewLine);
            richTextBox1.ScrollToCaret();
        }

        private void FormChat_Load(object sender, EventArgs e)
        {
            this.Text = "与" + this.name + "聊天";
            pb_header.Image = Image.FromFile(Application.StartupPath + "\\image\\" + imgindex.ToString() + ".jpg");
            label1.Text = this.name + "(" + this.hisnum + ")";
        }

        private void FormChat_FormClosing(object sender, FormClosingEventArgs e)
        {
            myInfo.ChatForm.Remove(hisnum);
        }

        private void skinbtn_send_Click(object sender, EventArgs e)
        {
            if (richTextBox2.Text != "") //输入框不空
            {
                //输入框没有图片//////////////////////////////////////////////////////////////////////
                {
                    str = richTextBox2.Text;
                    str = str.Insert(0, myname + " " + DateTime.Now.ToLongTimeString() + ":\n");
                    b_mes = mes(str);//转化为字节数组
                    all = new byte[b_mes.Length];
                    Array.Copy(b_mes, all, b_mes.Length);

                    string ALL = Encoding.Unicode.GetString(all, 0, all.Length);
                    ALL = ALL.Insert(0, "[talk][" + mynum + "," + hisnum + "]");
                    myInfo.MsgReadyToBeSentToServer = ALL; //发送文本

                    //将发送框内容显示到对话框中
                    richTextBox1.AppendText(myname + " " + DateTime.Now.ToLongTimeString() + ":\n" + richTextBox2.Text + Environment.NewLine);
                    richTextBox1.ScrollToCaret();
                }

                richTextBox2.Clear();
                richTextBox2.Focus();
                Array.Clear(b_mes, 0, b_mes.Length);
                Array.Clear(all, 0, all.Length);
            }
        }

    }
}
