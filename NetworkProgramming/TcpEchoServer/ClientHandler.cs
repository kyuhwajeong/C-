using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpEchoClient
{
    using System;

    using System.IO;

    using System.Text;

    using System.Net;

    using System.Net.Sockets;

    using System.Threading;




    public class ClientHandler
    {

        public Socket clientSocket;

        public void runClient()
        {

            NetworkStream stream = null;

            StreamReader reader = null;




            try
            {

                //client의 접속이 올때까지 block되는 부분(Thread)

                //백그라운드 thread에 처리 맡김

                //clientSocket = tcpListener.AcceptSocket();




                //클라이언트의 데이터를 읽고, 쓰기 위한 스트림을 만든다

                stream = new NetworkStream(clientSocket);









                Encoding encode = System.Text.Encoding.GetEncoding("ks_c_5601-1987");

                reader = new StreamReader(stream, encode);




                while (true)
                {

                    string str = reader.ReadLine();

                    if (str.IndexOf("<EOF>") > -1)
                    {

                        Console.WriteLine("Bye Bye");

                        break;

                    }

                    Console.WriteLine(str);

                    str += "\r\n";




                    byte[] dataWrite = Encoding.Default.GetBytes(str);




                    stream.Write(dataWrite, 0, dataWrite.Length);

                }

            }

            catch (Exception e)
            {

                Console.WriteLine(e.ToString());

            }

            finally
            {

                clientSocket.Close();

            }

        }

    }
}
