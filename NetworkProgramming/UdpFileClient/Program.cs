using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UdpFileClient
{
    class Program
    {
 static void Main(string[] args)
        {

            string filenametouse = @"c:\eula.1031.txt";
            try
            {

                IPAddress[] ipAddress = Dns.GetHostAddresses("");
                IPEndPoint ipEnd = new IPEndPoint(ipAddress[0], 5656);
                Socket clientsocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

                byte[] filenamedata = Encoding.ASCII.GetBytes(filenametouse);
                byte[] output = new byte[4 + filenamedata.Length];
                BitConverter.GetBytes(filenamedata.Length).CopyTo(output, 0);
                filenamedata.CopyTo(output, 4);
                //clientsocket.Connect(ipEnd);
                 clientsocket.Connect("localhost",5656);
                Console.WriteLine("Connected..");
                clientsocket.SendFile(filenametouse, output, null, TransmitFileOptions.UseDefaultWorkerThread);
                Console.WriteLine("File : {0} sent.", filenametouse);

                clientsocket.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine("File receiving error :" + ex.Message.ToString());
            }

            Console.WriteLine("Press a key");
            Console.ReadKey();
        }

    }
}
