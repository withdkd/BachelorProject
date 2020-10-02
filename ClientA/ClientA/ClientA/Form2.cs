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

namespace ClientA
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            Preload();
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
            using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + @"/socket.ini"))
            {
                sw.Write("ipserver=" + textBox1.Text + ";" + "ipcheck=" + textBox2.Text + "; ");
            }
            MessageBox.Show("Чтобы изменения вступили в силу, перезапустите приложение.");
            this.Close();
        }
    }
}
