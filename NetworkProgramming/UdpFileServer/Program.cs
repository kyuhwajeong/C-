using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UdpFileServer
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                IPEndPoint ipEnd = new IPEndPoint(IPAddress.Any, 5656);
                Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

                server.Bind(ipEnd);
                server.Listen(10);

                Console.WriteLine("Waiting for client...");
                Socket serverSocket = server.Accept();

                byte[] data = new byte[1024 * 5000];
                int received = serverSocket.Receive(data);
                int fileNameLen = BitConverter.ToInt32(data, 0);
                string fileName = Encoding.ASCII.GetString(data, 4, fileNameLen);
                //BinaryWriter bWrite = new BinaryWriter(File.Open(fileName, FileMode.Create, FileAccess.Write));
                BinaryWriter bWrite = new BinaryWriter(File.Create(@"C:\a\xxxx.txt"));
                bWrite.Write(data, fileNameLen + 4, received - fileNameLen - 4);
                int received2 = serverSocket.Receive(data);
                while (received2 > 0)
                {
                    bWrite.Write(data, 0, received2);
                    received2 = serverSocket.Receive(data);
                }
                bWrite.Close();
                serverSocket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending file." + ex.Message.ToString());
            }
            Console.Write("press a key");
            Console.ReadKey();
        }
    }
}
