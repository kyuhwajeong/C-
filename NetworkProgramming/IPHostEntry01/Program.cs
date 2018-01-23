using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IPHostEntry01
{
    class Program
    {
        static void Main(string[] args)
        {
            IPHostEntry HostInfo = Dns.GetHostEntry("www.naver.com");

            foreach (IPAddress ip in HostInfo.AddressList)
            {
                Console.WriteLine("{0}", ip);
            }

            Console.WriteLine("{0}", HostInfo.HostName);
            Console.ReadKey();
        }
    }
}
