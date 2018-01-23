using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpClientGetStream
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient tcpClient = new TcpClient("125.138.81.37", 7);
            if (tcpClient.Connected)
            {
                Console.WriteLine("서버 연결 성공");
                NetworkStream ns = tcpClient.GetStream();
                string Message = "Hello world";
                byte[] SendByteMessage = Encoding.ASCII.GetBytes(Message);
                ns.Write(SendByteMessage, 0, SendByteMessage.Length);

                byte[] ReceiveByteMessage = new byte[32];
                ns.Read(ReceiveByteMessage, 0, 32);
                string ReceiveMessage = Encoding.ASCII.GetString(ReceiveByteMessage);
                Console.WriteLine(ReceiveMessage);
                ns.Close();
            }
            else
            {
                Console.WriteLine("서버 연결 실패");
            }

            tcpClient.Close();
            Console.ReadKey();
        }
    }
}
