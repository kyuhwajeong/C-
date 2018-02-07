using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Controls;

using System.Windows.Forms.Integration;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            TextBlock tb = new TextBlock();
            tb.Text = "WPF TextBlock!!";
            elementHost1.Child = tb;           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ElementHost host = new ElementHost();




            System.Windows.Controls.Button btn = new System.Windows.Controls.Button();

            btn.Content = "WPF Button!!";



            host.Child = btn;

            host.Location = new Point(45, 10);



            this.Controls.Add(host); 

        }
    }
}
