using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AccetTcpClient01
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Parse("125.138.81.37"), 7);
            tcpListener.Start();
            Console.WriteLine("대기 상태 시작");
            TcpClient tcpClient = tcpListener.AcceptTcpClient();
            tcpListener.Stop();
            Console.WriteLine("대기 상태 종료");
            Console.ReadKey();
        }
    }
}
