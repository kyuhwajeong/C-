using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpClient01
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient tcpClient = new TcpClient("125.138.81.37", 7);
            if (tcpClient.Connected)
                Console.WriteLine("서버 연결 성공");
            else
                Console.WriteLine("서버 연결 싶패");

            tcpClient.Close();
            Console.ReadKey();
        }
    }
}
