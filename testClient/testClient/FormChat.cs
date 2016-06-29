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
    public delegate void UpdateFormChat(string msg);

    public partial class FormChat : CCSkinMain
    {
        string myAccount = string.Empty; //当前用户的账号
        string toUser = string.Empty; //发送给好友的好友账号
        
        string sendToServerMsg; //发送给服务器的消息
        string myname; //我的昵称

        string name; //聊天对象的昵称
        int imgindex; //好友头像索引

        bool isCTRLPressed = false;

        public FormChat()
        {
            InitializeComponent();
        }

        public FormChat(string myAccount, string toUser, string myName, string toUser_Name, int imageindex)
        {
            InitializeComponent();
            this.myAccount = myAccount;
            this.toUser = toUser;
            this.myname = myName;
            this.name = toUser_Name;
            imgindex = imageindex; 
            myInfo.ChatForm.Add(toUser, this); //加入到ChatForm，表明此窗体已经打开，在显示托盘气球时需要判断
        }


        /// <summary>
        /// 通过FormMain中ProcessPersonalMsg调用，由传来的msg消息更新聊天窗口的richTextBox1
        /// </summary>
        /// <param name="msg"></param>
        public void updateFormChat(string msg)
        {
            richTextBox1.AppendText(msg + Environment.NewLine);
            richTextBox1.ScrollToCaret();
        }


        private void FormChat_Load(object sender, EventArgs e)
        {
            this.Text = "与" + this.name + "聊天";
            pb_header.Image = Image.FromFile(Application.StartupPath + "\\image\\" + imgindex.ToString() + ".jpg");
            label1.Text = this.name + "(" + this.toUser + ")";
        }

        private void FormChat_FormClosing(object sender, FormClosingEventArgs e)
        {
            myInfo.ChatForm.Remove(toUser);
        }

        private void skinbtn_send_Click(object sender, EventArgs e)
        {
            if (richTextBox2.Text != "") //输入框不空
            {
                {
                    string str = richTextBox2.Text; //实际聊天消息
                    str = str.Insert(0, myname + " " + DateTime.Now.ToLongTimeString() + ":\n"); //添加我的昵称和时间信息

                    sendToServerMsg = str.Insert(0, "[talk][" + myAccount + "," + toUser + "]"); //添加前缀组成实际的发送给服务器的消息

                    myInfo.MsgReadyToBeSentToServer = sendToServerMsg; //发送文本

                    //将发送框内容显示到对话框中
                    richTextBox1.AppendText(myname + " " + DateTime.Now.ToLongTimeString() + ":\n" + richTextBox2.Text + Environment.NewLine);
                    richTextBox1.ScrollToCaret();
                }

                richTextBox2.Clear();
                richTextBox2.Focus();

                sendToServerMsg = string.Empty;

            }
        }


        /*实现CTRL+ENTER发送消息*/
        /// <summary>
        /// 按回车前按CTRL键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBox2_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                isCTRLPressed = true;
            }
        }


        /// <summary>
        /// 输入框按CTRL+回车发送消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && isCTRLPressed == true)
            {
                this.skinbtn_send_Click(sender, e);
            }   
        }

        
        /// <summary>
        /// 释放回车后清空因回车键在richTextBox2造成的换行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && isCTRLPressed == true)
            {
                richTextBox2.Clear();
                richTextBox2.Focus();
                isCTRLPressed = false;
            }   
        }

    }
}
