using System;
using System.Collections.Generic;
using System.Text;

namespace testClient
{
    class SplitMsg
    {
        private string Msg;

        public string MSG
        {
            set { Msg = value; }
            get { return Msg; }
        }

        public string[] process()
        {
            List<string> splittedMsg = new List<string>();

            int splitindex = Msg.IndexOf("[e]");
            while (splitindex != -1)
            {
                splittedMsg.Add(Msg.Substring(0, splitindex));
                //MessageBox.Show(rawmsg.Substring(0,splitindex));
                Msg = Msg.Substring(splitindex + 3);
                splitindex = Msg.IndexOf("[e]");
            }

            return splittedMsg.ToArray();
        }
    
    }
}
