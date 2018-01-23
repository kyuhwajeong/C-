using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpEchoClient
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient client = null;


            try
            {
                //LocalHost에 지정포트로 TCP Connection생성 후 데이터 송수신 스트림 얻음

                client = new TcpClient();

                client.Connect("127.0.0.1", 5001);

                NetworkStream writeStream = client.GetStream();

                Encoding encode = System.Text.Encoding.GetEncoding("ks_c_5601-1987");
                StreamReader readerStream = new StreamReader(writeStream, encode);

                //보낼 데이터를 읽어 Default형식의 바이트 스트림으로 변환

                string dataToSend = Console.ReadLine();
                byte[] data = Encoding.Default.GetBytes(dataToSend);



                while (true)
                {

                    dataToSend += "\r\n";

                    data = Encoding.Default.GetBytes(dataToSend);

                    writeStream.Write(data, 0, data.Length);

                    if (dataToSend.IndexOf("<EOF>") > -1)
                        break;

                    string returnData;
                    returnData = readerStream.ReadLine();
                    Console.WriteLine("server : " + returnData);          
                    dataToSend = Console.ReadLine();

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }
            finally
            {
                client.Close();
            }

        }

    }
}
