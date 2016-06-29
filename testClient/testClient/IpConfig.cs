using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace testClient
{
    class IpConfig
    {
        //public static IPAddress Ip = IPAddress.Parse("127.0.0.1");    
        //public static IPAddress Ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[2];//ipv4
        static IPAddress[] IpList = Dns.GetHostAddresses(Dns.GetHostName());
        static IPAddress Ip;
        public static int Port = 12150;

        public static IPAddress getIpv4()
        {
            foreach (IPAddress ipa in IpList)
            {
                if (ipa.AddressFamily == AddressFamily.InterNetwork)
                {
                    Ip = ipa;
                    break;
                }
            }
            return Ip;
        }

    }
}
