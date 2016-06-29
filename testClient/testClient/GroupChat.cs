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
    public delegate void UpdateGroupChat(string msg);

    public partial class GroupChat : CCSkinMain
    {
        int imgindex;
        string myAccount; //当前用户的账号
        string groupid; //群组号

        string sendToServerMsg; //发送给服务器的消息

        string myname; //我的昵称
        string groupname; //群组名称

        public GroupChat()
        {
            InitializeComponent();
        }

        public GroupChat(string myAccount, string groupid, string myname, string groupname, int imageindex)
        {
            InitializeComponent();
            this.myAccount = myAccount;
            this.groupid = groupid;
            this.myname = myname;
            this.groupname = groupname;
            imgindex = imageindex;
            myInfo.GroupChatForm.Add(groupid, this); //说明groupid的聊天界面已打开
        }


        /// <summary>
        /// 通过FormMain中ProcessGroupMsg调用，由传来的msg消息更新群组聊天窗口的richTextBox1
        /// </summary>
        /// <param name="msg"></param>
        public void updateGroupChat(string msg)
        {
            richTextBox1.AppendText(msg + Environment.NewLine);
            richTextBox1.ScrollToCaret();
        }

        private void GroupChat_Load(object sender, EventArgs e)
        {
            this.Text = "群组聊天";
            pb_header.Image = Image.FromFile(Application.StartupPath + "\\image\\" + imgindex.ToString() + ".jpg");
            label1.Text = this.groupname + "(" + this.groupid + ")";
        }

        private void GroupChat_FormClosing(object sender, FormClosingEventArgs e)
        {
            myInfo.GroupChatForm.Remove(groupid);//说明groupid的聊天界面已关闭
        }

        private void skinbtn_send_Click(object sender, EventArgs e)
        {
            if (richTextBox2.Text != "") //输入框不空
            {
                {
                    string str = richTextBox2.Text; //实际聊天消息
                    str = str.Insert(0, myname + " " + DateTime.Now.ToLongTimeString() + ":\n"); //添加我的昵称和时间信息

                    sendToServerMsg = str.Insert(0, "[group][" + myAccount + "," + groupid + "]"); //添加前缀组成实际的发送给服务器的消息

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


    }
}
