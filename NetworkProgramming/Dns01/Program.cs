using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Dns01
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress[] IP = Dns.GetHostAddresses("www.naver.com");
            foreach(IPAddress HostIP in IP)
            {
                Console.WriteLine("{0}", HostIP);
            }
        }
    }
}
