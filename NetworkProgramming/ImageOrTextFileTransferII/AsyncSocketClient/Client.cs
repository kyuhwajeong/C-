using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace AsyncSocketClient
{
    enum Commands
    {
        String = 0,
        Image
    }

    public class Client
    {
        public delegate void OnConnectEventHandler(Client sender, bool connected);
        public event OnConnectEventHandler OnConnect;

        public delegate void OnSendEventHandler(Client sender, int sent);
        public event OnSendEventHandler OnSend;

        public delegate void OnDisconnectEventHandler(Client sender);
        public event OnDisconnectEventHandler OnDisconnect;

        public delegate void OnReceiveEventHandler(Client client, string text);
        public event OnReceiveEventHandler OnReceive;

        public delegate void OnDisconnectByServerEventHandler(Client sender);
        public event OnDisconnectByServerEventHandler OnDisconnectByServer;

        Socket socket;
        string responseText = string.Empty;

        public bool Connected
        {
            get
            {
                if (socket != null)
                {
                    return socket.Connected;
                }

                return false;
            }
        }

        public Client()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect(string ipAddress, int port)
        {
            try
            {
                if (socket == null)
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                socket.Connect(IPAddress.Parse(ipAddress), port);

                if (OnConnect != null)
                {
                    OnConnect(this, Connected);
                }

                Receive();
                // callback 함수를 이용한 비동기 호출로 connect 연결함
                //socket.BeginConnect(ipAddress, port, connectCallBack, null);
            }
            catch (SocketException se)
            {
                throw se;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void connectCallBack(IAsyncResult ar)
        {
            try
            {
                socket.EndConnect(ar);
        
                if(OnConnect != null)
                {
                    OnConnect(this, Connected);
                }
            }
            catch (SocketException se) 
            {
                throw se;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Send(byte[] data, int index, int length)
        {
            socket.BeginSend(BitConverter.GetBytes(length), 0, 4, SocketFlags.None, sendCallBack, null);
            System.Threading.Thread.Sleep(500);
            socket.BeginSend(data, index, length, SocketFlags.None, sendCallBack, null);
        }

        public void sendCallBack(IAsyncResult ar)
        {
            try
            {
                int sent = socket.EndSend(ar);

                if (OnSend != null)
                {
                    OnSend(this, sent);
                }
            }
            catch(Exception ex) 
            {
                Trace.WriteLine(string.Format("SEND ERROR\n{0}", ex.Message));
            }
        }

        public void Disconnect()
        {
            try
            {
                if (socket.Connected)
                {
                    socket.Close();
                    socket = null;
                    if (OnDisconnect != null)
                    {
                        OnDisconnect(this);
                    }
                }
            }
            catch { }
        }

        private void Receive()
        {
            StateObject state = new StateObject();
            state.workSocket = socket;

            socket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            StateObject state = ar.AsyncState as StateObject;

            try
            { 
                int byteRead = state.workSocket.EndReceive(ar);

                if (byteRead > 0)
                {
                    state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, byteRead));
                    socket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    if (state.sb.Length >= 1)
                        responseText = state.sb.ToString();
                    else
                    {
                        this.Disconnect();
                        if (OnDisconnectByServer != null)
                        {
                            OnDisconnectByServer(this);
                        }
                    }
                }
            }
            catch(SocketException se)
            {
                this.Disconnect();

                switch(se.SocketErrorCode)
                {
                    case SocketError.ConnectionAborted:
                    case SocketError.ConnectionRefused:
                    case SocketError.ConnectionReset:
                        if (OnDisconnectByServer != null)
                        {
                            OnDisconnectByServer(this);
                            return;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("Error : {0}", ex.Message));
            }
        }
    }
}
