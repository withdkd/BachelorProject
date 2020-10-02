using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.IO;


namespace ClientB
{
    public partial class Form1 : Form
    {
        public delegate void Change(string all);
        public static int uniqProcessId;
        public static int port_in = 50002;
        public static int port_out = 50001;
        private static DateTime t;
        public static string This_ip
        {
            get; set;
        }
        public static string Server_ip
        {
            get; set;
        }
        public Form1()
        {
            InitializeComponent();
            Preload();
            uniqProcessId = Process.GetCurrentProcess().Id;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                MessageBox.Show("Временная метка не обнаружена, файл не был защищен.");
            }
            //if (listBox1.SelectedIndex != -1)
            //{
            //    int selectedIndex = listBox1.SelectedIndex;
            //    string item = listBox1.SelectedItem.ToString().Split('[')[1].Split(']')[0]; //?
            //    DateTime date = DateTime.Parse(item);
            //    Other obj = Database.PullFromDb(date);
            //    if (obj != null)
            //        MessageBox.Show(obj.date.ToString() + obj.result);
            //    else
            //        Debug.Log("obj == null");                
            //}
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
                Server_ip = all.Split(';')[0].Split('=')[1];
                This_ip = all.Split(';')[1].Split('=')[1];
            }
            else
            {
                MessageBox.Show("Конфигурация сети не задана.");
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Thread server = new Thread(AwaitConnections);            
            server.Start();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Process process = Process.GetProcessById(uniqProcessId);
            process.Kill();            
        }
        private void AwaitConnections()
        {            
            try
            {                
                IPEndPoint point = new IPEndPoint(IPAddress.Parse(This_ip), port_in);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(point);
                socket.Listen(10);
                while (true)
                {
                    Socket handler = socket.Accept();
                    int bytes = 0;
                    byte[] data = new byte[256];
                    List<byte> vs = new List<byte>();
                    do
                    {
                        bytes = handler.Receive(data);
                        vs.AddRange(data);
                    }
                    while (handler.Available > 0);
                    if (vs.Count > 0)
                    {
                        byte[] get = new byte[vs.Count];
                        for (int i = 0; i < vs.Count; i++)
                            get[i] = vs[i];
                        string md5 = Cryptography.GetMD5(get);                        
                        Other obj = new Other
                        {
                            md5 = md5,
                            date = DateTime.Now,
                            result = SendInfo(get)
                        };                                                
                        //Database.InsertIntoDb(obj);
                        string all = "[" + obj.date.ToString() + "]";
                        BeginInvoke(new Change(ChangeElement), all);
                    }
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }

            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }
        public void ChangeElement(string all)
        {
            listBox1.Items.Add(all);
        }
        private string SendInfo(byte[] src)
        {
            string answer = "";
            try
            {
                IPEndPoint point = new IPEndPoint(IPAddress.Parse(Server_ip), port_out);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(point);
                socket.Send(src);
                byte[] data = new byte[256];
                StringBuilder sb = new StringBuilder();
                int bytes = 0;
                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    sb.Append(Encoding.Unicode.GetString(data, 0, bytes));
                } while (socket.Available > 0);
                answer = sb.ToString();
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
            //t = DateTime.Parse(answer);
            if (answer == "_Negative_")
                return "Отрицательно";
            else
                return answer;
            
        }

        private void сетьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.Show();
        }

        
    }
}
