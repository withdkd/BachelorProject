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

namespace ClientB
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            
        }

        private void Preload()
        {
            if (File.Exists(Directory.GetCurrentDirectory() + @"/socket.ini"))
            {
                string all = "";
                using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + @"/socket.ini"))
                {
                    all = sr.ReadToEnd();
                }
                textBox1.Text = all.Split(';')[0].Split('=')[1];
                textBox2.Text = all.Split(';')[1].Split('=')[1];
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + @"/socket.ini"))
                {
                    sw.Write("ipserver=" + textBox1.Text + ";" + "this_ip=" + textBox2.Text + "; ");
                }
                MessageBox.Show("Чтобы изменения вступили в силу, перезапустите приложение.");
                this.Close();
            }
            else
            {
                MessageBox.Show("Для корректной работы приложения, поля должны содержать соответствующие значения.");
            }
        }
    }
}
