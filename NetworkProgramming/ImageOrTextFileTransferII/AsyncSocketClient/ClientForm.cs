using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;

namespace AsyncSocketClient
{
    public partial class ClientForm : Form
    {
        Client client;
        public ClientForm()
        {
            InitializeComponent();

            client = new Client();
            client.OnConnect += new Client.OnConnectEventHandler(client_OnConnect);
            client.OnSend += new Client.OnSendEventHandler(client_OnSend);
            client.OnDisconnect += new Client.OnDisconnectEventHandler(client_OnDisconnect);
            client.OnDisconnectByServer += new Client.OnDisconnectByServerEventHandler(client_OnDisconnectByServer);
        }

        void client_OnDisconnectByServer(Client sender)
        {
            MessageBox.Show("서버로부터 연결이 끊겼습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Stop);
 
            btnConnect.Enabled = true;
            btnSendText.Enabled = false;
            btnSendImage.Enabled = false;
            btnDisconnect.Enabled = false;

            toolStripStatusLabel1.Text = "Disconnected";
            toolStripStatusLabel2.Text = "";
        }

        void client_OnDisconnect(Client sender)
        {
            toolStripStatusLabel1.Text = "Disconnected";
        }

        void client_OnSend(Client sender, int sent)
        {
            Invoke((MethodInvoker)delegate
            {
                toolStripStatusLabel2.Text = string.Format("Data Sent:{0}", sent);
            });
        }

        void client_OnConnect(Client sender, bool connected)
        {
            if (connected)
                toolStripStatusLabel1.Text = "Connected";
        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            { 
                if (!client.Connected)
                {
                    client.Connect("127.0.0.1", 8192);

                    btnConnect.Enabled = false;
                    btnSendText.Enabled = true;
                    btnSendImage.Enabled = true;
                    btnDisconnect.Enabled = true;
                }
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSendText_Click(object sender, EventArgs e)
        {
            if (client.Connected)
                SendText(textBox1.Text);
            else
                toolStripStatusLabel1.Text = "서버 접속을 하시기 바랍니다. !!!";
        }

        void SendText(string text)
        {
            BinaryWriter bw = new BinaryWriter(new MemoryStream());
            bw.Write((int)Commands.String);
            bw.Write(text);
            byte[] data = ((MemoryStream)bw.BaseStream).ToArray();
            bw.BaseStream.Dispose();
            client.Send(data, 0, data.Length);
            data = null;
        }

        void SendImage(string path)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            byte[] b = File.ReadAllBytes(path);
            bw.Write((int)Commands.Image);
            bw.Write((int)b.Length);
            bw.Write(b);
            bw.Close();
            b = ms.ToArray();
            ms.Dispose();

            client.Send(b, 0, b.Length);
        }

        private void btnSendImage_Click(object sender, EventArgs e)
        {
            if (client.Connected)
            {
                using (OpenFileDialog o = new OpenFileDialog())
                {
                    o.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                    o.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    if (o.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        SendImage(o.FileName);
                    }
                }
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (client.Connected)
            {
                client.Disconnect();

                btnConnect.Enabled = true;
                btnSendText.Enabled = false;
                btnSendImage.Enabled = false;
                btnDisconnect.Enabled = false;

                toolStripStatusLabel1.Text = "Disconnected";
                toolStripStatusLabel2.Text = "";
            }
        }
    }
}
