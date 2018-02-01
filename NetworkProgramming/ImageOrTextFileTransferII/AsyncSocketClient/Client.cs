using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.IO;

namespace AsyncSocketClient
{
    enum Commands
    {
        String = 0,
        Image
    }

     #region ServerReceive define
        public struct ReceiveBuffer
        {
            public const int BUFFER_SIZE = 1024;
            public byte[] Buffer;
            public int ToReceive;
            public MemoryStream BufStream;

            public ReceiveBuffer(int toRec)
            {
                Buffer = new byte[BUFFER_SIZE];
                ToReceive = toRec;
                BufStream = new MemoryStream(toRec);
            }

            public void Dispose()
            {
                Buffer = null;
                ToReceive = 0;
                Close();
                if (BufStream != null)
                    BufStream.Dispose();
            }

            public void Close()
            {
                if (BufStream != null && BufStream.CanWrite)
                    BufStream.Close();
            }
        }
        #endregion

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
 
        #region ServerReceive define
        public delegate void DataReceivedEventHandler(Client sender, ReceiveBuffer e);
        public event DataReceivedEventHandler DataReceived;
        #endregion

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
            #region ServerReceive
            lenBuffer = new byte[4];
            #endregion
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
                    #region ServerReceive define
                    DataReceived = null;
                    lenBuffer = null;
                    #endregion
                }
            }
            catch { }
        }

        #region ServerReceive
        byte[] lenBuffer;
        ReceiveBuffer buffer;
        public void ReceiveAsync()
        {
            socket.BeginReceive(lenBuffer, 0, lenBuffer.Length, SocketFlags.None, ReceiveCallback, null);
        }

        public void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                int rec = socket.EndReceive(ar);

                if (rec == 0)
                {
                    Disconnect();
                    return;
                }

                if (rec != 4)
                {
                    throw new Exception();
                }
            }
            catch (SocketException se)
            {
                switch (se.SocketErrorCode)
                {
                    case SocketError.ConnectionAborted:
                    case SocketError.ConnectionReset:
                        Disconnect();
                        return;
//                        break;
                }
            }
            catch (ObjectDisposedException)
            {
                return;
            }
            catch (NullReferenceException)
            {
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            buffer = new ReceiveBuffer(BitConverter.ToInt32(lenBuffer, 0));

            socket.BeginReceive(buffer.Buffer, 0, buffer.Buffer.Length, SocketFlags.None, receivePacketCallBack, null);
        }

        public void receivePacketCallBack(IAsyncResult ar)
        {
            int rec = socket.EndReceive(ar);

            if (rec <= 0)
            {
                return;
            }

            buffer.BufStream.Write(buffer.Buffer, 0, rec);

            buffer.ToReceive -= rec;

            if (buffer.ToReceive > 0)
            {
                Array.Clear(buffer.Buffer, 0, buffer.Buffer.Length);

                socket.BeginReceive(buffer.Buffer, 0, buffer.Buffer.Length, SocketFlags.None, receivePacketCallBack, null);
                return;
            }

            if (DataReceived != null)
            {
                buffer.BufStream.Position = 0;
                DataReceived(this, buffer);
            }

            buffer.Dispose();

            ReceiveAsync();
        }

        #endregion
        
        private void Receive()
        {
            StateObject state = new StateObject();
            state.workSocket = socket;

            socket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
        }

        //private void ReceiveCallback(IAsyncResult ar)
        //{
        //    StateObject state = ar.AsyncState as StateObject;

        //    try
        //    { 
        //        int byteRead = state.workSocket.EndReceive(ar);

        //        if (byteRead > 0)
        //        {
        //            state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, byteRead));
        //            socket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
        //        }
        //        else
        //        {
        //            if (state.sb.Length >= 1)
        //                responseText = state.sb.ToString();
        //            else
        //            {
        //                this.Disconnect();
        //                if (OnDisconnectByServer != null)
        //                {
        //                    OnDisconnectByServer(this);
        //                }
        //            }
        //        }
        //    }
        //    catch(SocketException se)
        //    {
        //        this.Disconnect();

        //        switch(se.SocketErrorCode)
        //        {
        //            case SocketError.ConnectionAborted:
        //            case SocketError.ConnectionRefused:
        //            case SocketError.ConnectionReset:
        //                if (OnDisconnectByServer != null)
        //                {
        //                    OnDisconnectByServer(this);
        //                    return;
        //                }
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Trace.WriteLine(string.Format("Error : {0}", ex.Message));
        //    }
        //}
    }
}
