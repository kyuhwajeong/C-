using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MSSocketASync01Client_WindowsForm
{
    class Client
    {
        public delegate void OnConnectEventHandler(Client sender, bool connected);
        public event OnConnectEventHandler OnConnect;

        public delegate void OnSendEventHandler(Client sender, int bytesSent);
        public event OnSendEventHandler OnSend;

        public delegate void OnReceiveEventHandler(string receiveData);
        public event OnReceiveEventHandler OnReceive;

        public delegate void OnDisconnectEventHandler(Client sender);
        public event OnDisconnectEventHandler OnDisconnect;

        // The port number for the remote device
        private const int port = 11000;
        private string tcpServerIP = string.Empty;

        // ManualResetEvent instances signal completion.
        private ManualResetEvent connectDone = new ManualResetEvent(false);
        private ManualResetEvent sendDone = new ManualResetEvent(false);
        private ManualResetEvent receiveDone = new ManualResetEvent(false);

        // The response from the remote device.
        private string response = string.Empty;

        // Client socket define
        Socket client = null;

        public bool Connected
        {
            get
            {
                if (client != null)
                {
                    return client.Connected;
                }

                return false;
            }
        }

        public Client(string serverIP)
        {
            // Create a TCP/IP socket
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tcpServerIP = serverIP;
        }

        public void Close()
        {
            if (client.Connected)
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
                client = null;
                if (OnDisconnect != null)
                {
                    OnDisconnect(this);
                }
            }
        }

        public void StartClient(string sendData)
        {
            // Establish the remote endpoint for the socket
            // The Ip of the
            // remote device is "192.168.0.12" for test...
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(tcpServerIP), port);
            
            // Connect to the remote endpoint
            client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
            connectDone.WaitOne();

            // Send user input data to the remote device.
            sendData += "<EOF>";
            Send(sendData);
            sendDone.WaitOne();

            // Receive the response from the remote device.
            Receive(client);
            receiveDone.WaitOne();

            // Release the socket
            Close();
        }

        private void Receive(Socket client)
        {
            try
            {
                // Create the state object.
                StateObject state = new StateObject();
                state.workSocket = client;

                // Begin receiving the data from the remote device.
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket 
                // from the asynchronous state object.
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device.
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.
                    state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesRead));

                    // Get the rest of the data.
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    // All the data has arrived; put it in response.
                    if (state.sb.Length > 1)
                    {
                        response = state.sb.ToString();

                        if (OnReceive != null)
                        {
                            OnReceive(response);
                        }
                    }
                    // Signal that all bytes have been received.
                    receiveDone.Set();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void Send(string data)
        {
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = Encoding.UTF8.GetBytes(data);

            // Begin sending the data to the remote device.
            client.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), client);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = client.EndSend(ar);

                if (OnSend != null)
                {
                    OnSend(this, bytesSent);
                }                

                // Signal that all bytes have been sent.
                sendDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object
                Socket client = ar.AsyncState as Socket;

                // Complete the connection
                client.EndConnect(ar);

                if (OnConnect != null)
                {
                    OnConnect(this, Connected);
                }

                // Signal that the connection has been made
                connectDone.Set();
            }
            catch (SocketException se)
            {
                Console.WriteLine("ConnectCallback [SocketException] Error : {0} ", se.Message.ToString());
            }
            catch (Exception ex)
            {
                Console.Write("ConnectCallback [Exception] Error : {0} ", ex.Message.ToString());
            }
        }
    }

    // State object for receiving data from remote device
    public class StateObject
    {
        // Client socket
        public Socket workSocket = null;
        // Size of receive buffer
        public const int BufferSize = 256;
        // Receive buffer
        public byte[] buffer = new byte[BufferSize];
        // Receiving data string
        public StringBuilder sb = new StringBuilder();
    }
}
