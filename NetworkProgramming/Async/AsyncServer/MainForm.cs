using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncServer
{

    public partial class MainForm : Form
    {
        // Thread signal.
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        // Golobal Sockeet define for listener 
        public Socket g_listener = null;

        public MainForm()
        {
            InitializeComponent();
            FormClosing += new FormClosingEventHandler(MainForm_FormClosing);
        }

        public void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (g_listener != null)
            {
                g_listener.Close();
                g_listener.Dispose();
            }

            Application.Exit();
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            // button disabled.
            btnListen.Enabled = false;

            // Data buffer for incoming data
            byte[] bytes = new byte[10240];

            // Establish the local endpoint for the socket
            // The ip address of the computer
            // running the listener is "192.168.0.12" 예제로...
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(LocalIPAddress()), 11000);

            // Create a TCP/IP socket
            g_listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                g_listener.Bind(localEndPoint);
                g_listener.Listen(100);

                new Thread(delegate()
                    {
                        while (true)
                        {
                            // Set the event to nonsignaled state
                            allDone.Reset();

                            // Start an asynchronous socket to listen for connections.
                            Invoke((MethodInvoker)delegate
                            {
                                listBox1.Items.Add("Waiting for a connection...");
                            });
                            g_listener.BeginAccept(new AsyncCallback(AcceptCallback), g_listener);

                            // Waiting until a connection is made before continuing.
                            allDone.WaitOne();
                        }
                    }).Start();
            }
            catch (SocketException se)
            {
                MessageBox.Show(string.Format("StartListening [SocketException] Error : {0} ", se.Message.ToString()));
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("StartListening [Exception] Error : {0} ", ex.Message.ToString()));
            }
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue
            allDone.Set();

            // Get the socket that handles the client request.
            Socket listener = ar.AsyncState as Socket;
            Socket handler = listener.EndAccept(ar);

            // Create the state object
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(ReadCallback), state);
        }

        public void ReadCallback(IAsyncResult ar)
        {
            string content = string.Empty;

            // Retrieve the state object and the handler socjet
            // from the asynchronous state object
            StateObject state = ar.AsyncState as StateObject;
            Socket handler = state.workSocket;

            // Read data from the client socket
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There might be more data, so store the data received so far
                state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesRead));

                // Check for end-of-flag tag. If it is no there, 
                // Read more data
                content = state.sb.ToString();
                if (content.IndexOf("<EOF>") > -1)
                {
                    // All the data has been read from the
                    // client. Display it on the console.
                    Invoke((MethodInvoker)delegate
                    {
                        content = TruncateLeft(content, content.Length - 5);
                        listBox1.Items.Add(string.Format("Read {0} bytes from socket. Data : {1}", content.Length, content));
                    });

                    // Echo the data back to the client
                    Send(handler, content);
                }
                else
                {
                    // Not all data received. Get more
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(ReadCallback), state);
                }
            }
        }

        public void Send(Socket handler, string data)
        {
            // Convert the string data byte data using UTF8 encoding.
            byte[] byteData = Encoding.UTF8.GetBytes(data);

            // Begin sending the data to the remote device.
            handler.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(SendCallback), handler);
        }

        public void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object
                Socket handler = ar.AsyncState as Socket;

                // Complete sending the data to remote device
                int bytesSent = handler.EndSend(ar);
                Invoke((MethodInvoker)delegate
                {
                    listBox1.Items.Add(string.Format("Sent {0} bytes to client", bytesSent));
                    listBox1.Items.Add("");
                });

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (SocketException se)
            {
                MessageBox.Show(string.Format("SendCallback [SocketException] Error : {0} ", se.Message.ToString()));
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("SendCallback [Exception] Error : {0} ", ex.Message.ToString()));
            }
        }

        public string TruncateLeft(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        // Get local IP
        public string LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    return localIP;
                }
            }
            return "127.0.0.1";
        }

        private void btnDataClear_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }
    }

    // State object for reading client data asynchronously
    public class StateObject
    {
        // Client socket
        public Socket workSocket = null;
        // Size of receive buffer
        public const int BufferSize = 1024;
        // Receive buffer
        public byte[] buffer = new byte[BufferSize];
        // Received data string
        public StringBuilder sb = new StringBuilder();
    }

}

