using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TcpEchoClient;

namespace TcpEchoServer
{
    class Program
    {
         public static void Main(string[] args)

    {

        TcpListener tcpListener = null;




        try

        {

            //ip주소를 나타내는 객체 생성. TcpListener생성시 인자로 사용

            IPAddress ipAd = IPAddress.Parse("127.0.0.1");




            //TcpListener class를 이용하여 클라이언트 연결 받아 들임

            tcpListener = new TcpListener(ipAd, 5001);

            tcpListener.Start();




            Console.WriteLine("멀티스레드 Test 창 :Waiting for connections...");




            while (true)

            {

                //accepting the connection

                Socket client = tcpListener.AcceptSocket();

                ClientHandler cHandler = new ClientHandler();

                //passing calue to the thread class

                cHandler.clientSocket = client;

                //creating a new thread for the client

                Thread clientThread = new Thread(new ThreadStart(cHandler.runClient));

                clientThread.Start();

            }

        }

        catch (Exception exp)

        {

            Console.WriteLine("Exception :" + exp);

        }

        finally

        {

            tcpListener.Stop();

        }

    }

    }
}
