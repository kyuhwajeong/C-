using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;



namespace AsyncSocketServer
{
    public partial class MainForm : Form
    {
        Listener listener;
        static List<Client> ClientList;

        public MainForm()
        {
            InitializeComponent();

            btnListen.Click += new EventHandler(btnListen_Click);
            btnClose.Click += new EventHandler(btnClose_Click);
            FormClosing += new FormClosingEventHandler(MainForm_FormClosing);

            ClientList = new List<Client>();
        }

        void btnListen_Click(object sender, EventArgs e)
        {
            listener = new Listener();
            listener.Accepted += new Listener.SocketAcceptedHandler(listener_Accepted);
            listener.Start(8192);

            Invoke((MethodInvoker)delegate
            {
                toolStripStatusLabel1.Text = "Server started!!!";
            });

            btnListen.Enabled = false;
            btnClose.Enabled = true;
        }

        void listener_Accepted(Socket e)
        {
            Client client = new Client(e);
            ClientList.Add(client);

            client.DataReceived += new Client.DataReceivedEventHandler(client_DataReceived);
            client.Disconnected += new Client.DisconnectedEventHandler(client_Disconnected);
            client.ReceiveAsync();

            Invoke((MethodInvoker) delegate{
                toolStripStatusLabel1.Text = "Connected: " + client.EndPoint.ToString();
            });
        }

        void client_Disconnected(Client sender)
        {
            sender.Close();
            sender = null;

            Invoke((MethodInvoker)delegate
            {
                toolStripStatusLabel1.Text = "Connected: ...";
                DialogResult res = MessageBox.Show("Client Disconnected\nClear Data?", "서버 메시지", MessageBoxButtons.YesNo);
                if (res == System.Windows.Forms.DialogResult.Yes)
                {
                    lstText.Items.Clear();
                    pbImage.Image = null;
                }
            });
        }

        void client_DataReceived(Client sender, ReceiveBuffer e)
        {
            BinaryReader r = new BinaryReader(e.BufStream);

            Commands header = (Commands)r.ReadInt32();
            
            switch (header)
            {
                case Commands.String:
                    {
                        string s = r.ReadString();
                        Invoke((MethodInvoker)delegate
                        {
                            lstText.Items.Add(s);
                        });
                    }
                    break;
                case Commands.Image:
                    {
                        int imageBytesLen = r.ReadInt32();

                        byte[] iBytes = r.ReadBytes(imageBytesLen);

                        Invoke((MethodInvoker)delegate
                        {
                            pbImage.Image = Image.FromStream(new MemoryStream(iBytes));
                        });

                        iBytes = null;
                    }
                    break;
            }
        }

        void btnClose_Click(object sender, EventArgs e)
        {
            foreach(Client client in ClientList)
            {
                if (client != null)
                {
                    client.Close();
                }
            }
            ClientList.Clear();

            if (listener != null && listener.Running)
                listener.Stop();

            toolStripStatusLabel1.Text = "...";

            lstText.Items.Clear();
            pbImage.Image = null;

            btnListen.Enabled = true;
            btnClose.Enabled = false;
        }

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Client client in ClientList)
            {
                if (client != null)
                {
                    client.Close();
                }
            }
            ClientList.Clear();

            if (listener != null && listener.Running)
                listener.Stop();
        }

        private void btnListen2_Click(object sender, EventArgs e)
        {
            listener = new Listener();
            listener.Accepted += new Listener.SocketAcceptedHandler(listener_Accepted);
            listener.Start(8192);

            Invoke((MethodInvoker)delegate
            {
                toolStripStatusLabel1.Text = "Server started!!!";
            });

            btnListen.Enabled = false;
            btnClose.Enabled = true;
        }

        private void btnClose2_Click(object sender, EventArgs e)
        {
            foreach (Client client in ClientList)
            {
                if (client != null)
                {
                    client.Close();
                }
            }
            ClientList.Clear();

            if (listener != null && listener.Running)
                listener.Stop();

            toolStripStatusLabel1.Text = "...";

            lstText.Items.Clear();
            pbImage.Image = null;

            btnListen.Enabled = true;
            btnClose.Enabled = false;
        }
    }
}
