using MSSocketASync01Client_WindowsForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncClient
{
    public partial class MainForm : Form
    {
        private Client client;
        public MainForm()
        {
            InitializeComponent();
            FormClosing += new FormClosingEventHandler(MainForm_FormClosing);
        }

        private void client_OnDisconnect(Client sender)
        {
            UpdateUIListBoxInfo("Disconnected");
        }

        private void client_OnReceive(string receiveData)
        {
            string content = string.Format("receiveData : {0}", receiveData);
            UpdateUIListBoxInfo(content);
            UpdateUIListBoxInfo("");
        }

        private void client_OnSend(Client sender, int bytesSent)
        {
            string content = string.Format("Sent {0} bytes to server.", bytesSent);
            UpdateUIListBoxInfo(content);
        }

        private void client_OnConnect(Client sender, bool connected)
        {
            if (connected)
            {
                UpdateUIListBoxInfo("Connection Accepted.....");
            }
        }

        public void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (client.Connected)
                client.Close();

            Application.Exit();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            client = new Client(txtServerIP.Text);
            client.OnConnect += new Client.OnConnectEventHandler(client_OnConnect);
            client.OnSend += new Client.OnSendEventHandler(client_OnSend);
            client.OnReceive += new Client.OnReceiveEventHandler(client_OnReceive);
            client.OnDisconnect += new Client.OnDisconnectEventHandler(client_OnDisconnect);

            string sendData = txtContent.Text;
            client.StartClient(sendData);
        }

        private void UpdateUIListBoxInfo(string content)
        {
            if (listBox1.InvokeRequired)
            {
                listBox1.BeginInvoke(new MethodInvoker(delegate
                {
                    listBox1.Items.Add(content);
                }));
            }
            else
                listBox1.Items.Add(content);
        }
    }
}
