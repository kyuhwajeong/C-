using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IPEndPoint01
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress IPInfo = IPAddress.Parse("127.0.0.1");
            int port = 13;
            IPEndPoint EndPointInfo = new IPEndPoint(IPInfo, port);
            Console.WriteLine("ip: {0} port:{1}", EndPointInfo.Address, EndPointInfo.Port);
            Console.WriteLine(EndPointInfo.ToString());
            Console.ReadKey();
        }
    }
}
