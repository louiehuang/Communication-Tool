using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Xml;
using System.Collections;

namespace 消息分类
{
    public partial class Form1 : Form
    {

        public Hashtable UserMsg = new Hashtable();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string Readmsg = "[off][talk][101511]Shane 22:59:03:\nhello[e]" + 
                                            "[talk][100001]Alice 22:59:30:\nHi[e]" + 
                                            "[group][000001]Shane 22:59:35:\nIn Group[e]" + 
                                            "[talk][101511]Shane 22:59:42:\nFromShane[e]" + 
                                            "[group][000001]Alice 23:10:35:\nTsetGroup[e]";


            /* 分成[talk][101511]Shane 22:59:03:\nhello\nShane 22:59:42:\nFromShane\n
                  和[talk][100001]Alice 22:59:30:\nHi\n
            */

            List<string> RawMsg = new List<string>();
            string rawmsg = Readmsg.Substring(5);
            //MessageBox.Show(rawmsg);
            int splitindex = rawmsg.IndexOf("[e]");
            while (splitindex != -1)
            { 
                RawMsg.Add(rawmsg.Substring(0,splitindex));
                //MessageBox.Show(rawmsg.Substring(0,splitindex));
                rawmsg = rawmsg.Substring(splitindex + 3);
                splitindex = rawmsg.IndexOf("[e]");
            }

            /*
             RawMsg[0] = "[talk][101511]Shane 22:59:03:\nhello" 
             RawMsg[1] = "[talk][100001]Alice 22:59:30:\nHi" 
             RawMsg[2] = "[group][000001]Shance 22:59:35:\nIn Group"
             RawMsg[3] = "[talk][101511]Shane 22:59:42:\nFromShane"
             */

            int index = 0;
            List<string> ClassfiedMsg = new List<string>();
            foreach (string singlemsg in RawMsg)
            {
                int flag = 0;
                string account = singlemsg.StartsWith("[talk]") ? singlemsg.Substring(7, 6) : singlemsg.Substring(8, 6); ;
                foreach (DictionaryEntry msg in UserMsg)
                {
                    if (msg.Key.ToString() == account) //找到
                    {
                        flag = 1;
                        int pos = int.Parse(msg.Value.ToString());
                        string onemsg = string.Empty;

                        if (singlemsg.StartsWith("[talk]"))
                        {
                            onemsg = singlemsg.Substring(14) + "\n"; //Shane 22:59:03:\nhello\n
                        }
                        else if (singlemsg.StartsWith("[group]"))
                        {
                            onemsg = singlemsg.Substring(15) + "\n";
                        }
                        ClassfiedMsg[pos] += onemsg;

                        //MessageBox.Show("found: " + ClassfiedMsg[pos]);

                    }
                }
                if (flag == 0)//没有找到
                {
                    //singlemsg = "[talk][101511]Shane 22:59:03:\nhello" 
                    //singlemsg = '[group][000001]Shance 22:59:35:\nIn Group"
                    UserMsg.Add(account, index);
                    string onemsg = singlemsg + "\n";
                    ClassfiedMsg.Insert(index, onemsg);

                    //MessageBox.Show("First time: " + onemsg);
                    index++;
                }
            }


            foreach (string str in ClassfiedMsg)
            {
                MessageBox.Show(str);
            }


            //int index = 0;
            //while (m.Success)
            //{
            //    int flag = 0;
            //    string account = m.Result("$0"); //101511
            //    int singlemsgstart = m.Index + 6 + 1;
            //    int singlemsgend = Readmsg.IndexOf("[e]");
            //    //MessageBox.Show(singlemsgstart + "---" + singlemsgend);
            //    string str = Readmsg.Substring(singlemsgstart, singlemsgend - singlemsgstart);
            //    MessageBox.Show(str);

            //    /*

            //    //MessageBox.Show(account);

            //    //遍历UserMsg查询当前消息的发起用户是否已被分类
            //    foreach (DictionaryEntry msg in UserMsg)
            //    {
            //        if (msg.Key.ToString() == account) //找到
            //        {
            //            flag = 1;
            //            int pos = int.Parse(msg.Value.ToString());

            //            string onemsg = child.Attributes["msg"].Value + "\n";

            //            ClassfiedMsg[pos] += onemsg;

            //            MessageBox.Show("found: " + ClassfiedMsg[pos]);

            //        }
            //    }
            //    if (flag == 0)//没有找到
            //    {
            //        //[talk][101511]Shane 12:44:50: hello\n
            //        //"[off][talk][101511]Shane 22:59:03:\nhello[e]"
            //        //realms = "Shane 22:59:03:\nhello";
            //        UserMsg.Add(account, index);
            //        string realmsg = null;
            //        string onemsg = "[talk]" + "[" + account + "]" + realmsg + "\n";
            //        //onemsg = "[talk][101511]Shane 22:59:03:\nhello\n"
            //        ClassfiedMsg.Insert(index, onemsg);

            //        MessageBox.Show("First time: " + onemsg);
            //        index++;
            //    }

            //    */

            //    //MessageBox.Show(m.Index + "    " + m.Result("$0")); //12 101511     50 100001 ...

            //    m = m.NextMatch();
            //}







             /*
            string str = "Shanesf 11:05:39:\n hello";
            Regex reg = new Regex(@"\d{1,2}:\d{1,2}:\d{1,2}");
            Match m = reg.Match(str);
            while (m.Success)
            {
                MessageBox.Show(m.Result("$0"));
                m = m.NextMatch();
            }
             */





 //           /*离线消息，服务器传xml形式的字符串到客户端
 //* <Readmsg>
 //*      <talk>
 //*          <num account="101511" name="Shane" time="12:44:50" msg="hello"></num>
 //*          <num account="101511" name="Shane" time="12:45:08" msg="Hi"></num>
 //*          <num account="100001" name="Alice" time="12:45:33" msg="FromAlice"></num>
 //*          <num account="101511" name="Shane" time="12:46:00" msg="FromShaneAgain"></num>
 //*      </talk>
 //* </Readmsg>
 //*/
 //           //string Readmsg =
 //           //    "<Readmsg>" +
 //           //        "<talk>" +
 //           //            "<num account=\"101511\" name=\"Shane\" time=\"12:44:50\" msg=\"hello\"></num>" +
 //           //            "<num account=\"101511\" name=\"Shane\" time=\"12:45:08\" msg=\"Hi\"></num>" +
 //           //             "<num account=\"100001\" name=\"Alice\" time=\"12:45:33\" msg=\"FromAlice\"></num>" +
 //           //            "<num account=\"101511\" name=\"Shane\" time=\"12:46:00\" msg=\"FromShaneAgain\"></num>" +
 //           //        "</talk>" +
 //           //      "<group>" +
 //           //             "" + 
 //           //      "</group>" + 
 //           //    "</Readmsg>";


 //           string Readmsg =
 //   "<Readmsg>" +
 //       "<talk>" +
 //           "<num account=\"101511\" msg=\"Shane 12:44:50:\nhello\"></num>" +
 //           "<num account=\"101511\" msg=\"Shane 12:45:08:\nHi\"></num>" +
 //            "<num account=\"100001\" msg=\"Alice 12:45:33:\nFromAlice\"></num>" +
 //           "<num account=\"101511\" msg=\"Shane 12:46:00:\nFromShaneAgain\"></num>" +
 //       "</talk>" +
 //   "</Readmsg>";

 //           XmlDocument doc = new XmlDocument();
 //           doc.LoadXml(Readmsg);

 //           //[talk][101511]Shane 12:44:50:\nhello\nShane 12:45:08:\nHi\nShane12:46:00:\nFromShaneAgain\n
 //           //[talk][100001]Alice 12:45:33:\nFromAlice\n

 //           List<string> ClassfiedMsg = new List<string>();

 //           XmlNodeList xmlnodeList = doc.SelectSingleNode("Readmsg").ChildNodes;
 //           int index = 0;
 //           foreach (XmlNode SuperirNode in xmlnodeList) //查找talk标签<talk>
 //           {
 //               if (SuperirNode.Name == "talk")
 //               {
 //                   foreach (XmlNode child in SuperirNode.ChildNodes) //对talk标签内的消息分类<num>
 //                   {
 //                       int flag = 0;
 //                       string account = child.Attributes["account"].Value;
 //                       //MessageBox.Show(account);

 //                       /*遍历UserMsg查询当前消息的发起用户是否已被分类*/
 //                       foreach (DictionaryEntry msg in UserMsg)
 //                       {
 //                           if (msg.Key.ToString() == account) //找到
 //                           {
 //                               flag = 1;
 //                               int pos = int.Parse(msg.Value.ToString());

 //                               string onemsg = child.Attributes["msg"].Value + "\n";

 //                               ClassfiedMsg[pos] += onemsg;

 //                               MessageBox.Show("found: " + ClassfiedMsg[pos]);

 //                           }
 //                       }
 //                       if (flag == 0)//没有找到
 //                       {
 //                           //[talk][101511]Shane 12:44:50: hello\n
 //                           UserMsg.Add(account, index);
 //                           string onemsg = "[talk]" + "[" + account + "]" + child.Attributes["msg"].Value + "\n";
 //                           ClassfiedMsg.Insert(index, onemsg);

 //                           MessageBox.Show("First time: " + onemsg);
 //                           index++;
 //                       }
 //                   }

 //               }
 //           }

        }
    }
}