using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class MainForm : Form
    {
        string m_splitter = "'\\'";
        string m_fName = string.Empty;
        string[] m_split = null;
        byte[] m_clientData = null;
        enum DataPacketType { TEXT = 1, IMAGE };

        public MainForm()
        {
            InitializeComponent();

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            char[] delimeter = m_splitter.ToCharArray();

            openFileDialog1.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            openFileDialog1.ShowDialog();

            textBox1.Text = openFileDialog1.FileName;
            pictureBox1.ImageLocation = openFileDialog1.FileName;

            m_split = textBox1.Text.Split(delimeter);
            int limit = m_split.Length;

            m_fName = m_split[limit - 1].ToString();

            if (textBox1.Text != null)
                btnSend.Enabled = true;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Thread t_handler = new Thread(SendData);
            t_handler.IsBackground = true;
            t_handler.Start();
        }

        private void SendData()
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            byte[] fileName = Encoding.UTF8.GetBytes(m_fName);
            byte[] fileData = File.ReadAllBytes(textBox1.Text);
            byte[] fileNameLen = BitConverter.GetBytes(fileName.Length);
            byte[] fileType = BitConverter.GetBytes((int)DataPacketType.IMAGE);
            // IMAGE(4 byte) + 파일이름(4 byte) + 파일이름길이(4 byte) + 데이타 길이
            m_clientData = new byte[fileType.Length + 4 + fileName.Length + fileData.Length];

            fileType.CopyTo(m_clientData, 0);
            fileNameLen.CopyTo(m_clientData, 4);
            fileName.CopyTo(m_clientData, 8);
            fileData.CopyTo(m_clientData, 8 + fileName.Length);

            clientSocket.Connect(IPAddress.Parse(txtIPAddress.Text), 9050);
            clientSocket.Send(m_clientData);
            clientSocket.Close();
        }

        private void btnSendText_Click(object sender, EventArgs e)
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            byte[] textData = Encoding.UTF8.GetBytes(textBox2.Text);
            byte[] fileType = BitConverter.GetBytes((int)DataPacketType.TEXT);
            // TEXT(4 byte) + 데이타 길이
            m_clientData = new byte[fileType.Length + textData.Length];

            fileType.CopyTo(m_clientData, 0);
            textData.CopyTo(m_clientData, 4);

            clientSocket.Connect(IPAddress.Parse(txtIPAddress.Text), 9050);
            clientSocket.Send(m_clientData);
            clientSocket.Close();
        }

    }
}
