﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace AsyncSocketServer
{
    enum Commands : int
    {
        String = 0,
        Image
    }

    #region ReceiveBuffer define
    struct ReceiveBuffer
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

    #region Client define
    class Client
    {
        byte[] lenBuffer;
        ReceiveBuffer buffer;
        Socket socket;

        public IPEndPoint EndPoint
        {
            get
            {
                if (socket != null && socket.Connected)
                    return (IPEndPoint)socket.RemoteEndPoint;

                return new IPEndPoint(IPAddress.None, 0);
            }
        }

        public delegate void DisconnectedEventHandler(Client sender);
        public event DisconnectedEventHandler Disconnected;
        public delegate void DataReceivedEventHandler(Client sender, ReceiveBuffer e);
        public event DataReceivedEventHandler DataReceived;

        public Client(Socket s)
        {
            socket = s;
            lenBuffer = new byte[4];
        }
        
        public void Close()
        {
            if (socket != null)
            {
                socket.Disconnect(false);
                socket.Close();
            }

            buffer.Dispose();
            socket = null;
            lenBuffer = null;
            Disconnected = null;
            DataReceived = null;
        }

        public void ReceiveAsync()
        {
            socket.BeginReceive(lenBuffer, 0, lenBuffer.Length, SocketFlags.None, receiveCallBack, null);
        }

        public void receiveCallBack(IAsyncResult ar)
        {
            try
            {
                int rec = socket.EndReceive(ar);

                if (rec == 0)
                {
                    if (Disconnected != null)
                    {
                        Disconnected(this);
                        return;
                    }
                }

                if (rec != 4)
                {
                    throw new Exception();
                }
            }
            catch(SocketException se)
            {
                switch(se.SocketErrorCode)
                {
                    case SocketError.ConnectionAborted:
                    case SocketError.ConnectionReset:
                        if (Disconnected != null)
                        {
                            Disconnected(this);
                            return;
                        }
                        break;
                }
            }
            catch(ObjectDisposedException)
            {
                return;
            }
            catch(NullReferenceException)
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
    }
    #endregion

}
