using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace XmlReadWrite
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            XmlWrite();
        }

        void XmlWrite()
        {
            using (XmlWriter wr = XmlWriter.Create(@"C:\Temp\Emp.xml"))
            {
                wr.WriteStartDocument();
                wr.WriteStartElement("Employees");

                // Employee#1001
                wr.WriteStartElement("Employee");
                wr.WriteAttributeString("Id", "1001");  // attribute 쓰기
                wr.WriteElementString("Name", "Tim");   // Element 쓰기
                wr.WriteElementString("Dept", "Sales");
                wr.WriteEndElement();

                // Employee#1002
                wr.WriteStartElement("Employee");
                wr.WriteAttributeString("Id", "1002");
                wr.WriteElementString("Name", "John");
                wr.WriteElementString("Dept", "HR");
                wr.WriteEndElement();

                wr.WriteEndElement();
                wr.WriteEndDocument();
            }

        }

        void xmlRead()
        {
            using (XmlReader rd = XmlReader.Create(@"C:\Temp\Emp.xml"))
            {
                while (rd.Read())
                {
                    if (rd.IsStartElement())
                    {
                        if (rd.Name == "Employee")
                        {
                            // attribute 읽기                            
                            string id = rd["Id"]; // rd.GetAttribute("Id");

                            rd.Read();   // 다음 노드로 이동        

                            // Element 읽기
                            string name = rd.ReadElementContentAsString("Name", "");
                            string dept = rd.ReadElementContentAsString("Dept", "");

                            Console.WriteLine(id + "," + name + "," + dept);
                            txtXML.Text += String.Format("{0},{1},{2}", id, name, dept) + System.Environment.NewLine;
                        }
                    }
                }
            }

        }

        private void btnXMLWrite_Click(object sender, EventArgs e)
        {
            XmlWrite();
        }

        private void btnXMLRead_Click(object sender, EventArgs e)
        {
            xmlRead();

        }

        private void btnDOMWrite_Click(object sender, EventArgs e)
        {
            // Create the XmlDocument.  
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<book genre='novel' ISBN='1-861001-57-5'>" +
                        "<title>Pride And Prejudice</title>" +
                        "</book>");

            // Save the document to a file.  
            doc.Save("data.xml");  

        }

    }
}
