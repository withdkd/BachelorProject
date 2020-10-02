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
    public partial class Form1 : Form
    {
        public byte[] src;
        public byte[] send;
        public Form1()
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
                Network.Ip_server = all.Split(';')[0].Split('=')[1];
                Network.Ip_judge = all.Split(';')[1].Split('=')[1];
            }            
        }

        //open
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = openFileDialog1;
            dialog.FileName = "";
            dialog.InitialDirectory = Directory.GetCurrentDirectory();
            dialog.Multiselect = false;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                byte[] load = File.ReadAllBytes(dialog.FileName);                
                pictureBox1.Image = Image.FromFile(dialog.FileName);                
                src = Data.ReSaveImage(load, dialog.SafeFileName);
                send = Data.SelectStruct(src);                
            }
        }

        //defend
        private void button2_Click(object sender, EventArgs e)
        {
            Network.SendInfo(send);
            Data.SetFullImage();
            //Data.DeleteTmp();
        }

        //check
        private void button3_Click(object sender, EventArgs e)
        {
            byte[] source = Data.SelectFragmentForCheck(src);
            //Data.DeleteTmp();
            Network.SendToCheck(src);            
        }

        private void сетьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.Show(this);
        }
    }
}
