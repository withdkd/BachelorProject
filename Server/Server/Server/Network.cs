using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Drawing;

namespace Server
{
    internal static class Network
    {
        internal static string Ip
        {
            get; set;
        }
        internal static readonly int port_clients = 50000;
        internal static readonly int port_judge = 50001;
        private static DateTime t;

        internal static void ServerForClients()
        {            
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(Ip), port_clients);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(ipPoint);
                socket.Listen(10);
                Console.WriteLine("Доступен по адресу {0}:{1}", Ip, port_clients);
                while (true)
                {
                    Socket handler = socket.Accept();
                    Console.WriteLine("Входящее соедиение ...");
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
                        Other obj = new Other();
                        Bitmap bmp = Graphics.SetImage(obj, get);                        
                        handler.Send(Cryptography.Insert(bmp, obj, obj.Width, obj.Height));
                        t = obj.Date;
                        //Console.WriteLine("Inserted {0}", obj.Date);
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

        internal static void ServerForCheckers()
        {            
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(Ip), port_judge);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(ipPoint);
                socket.Listen(10);
                Console.WriteLine("Доступен по адресу {0}:{1}", Ip, port_judge);
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
                        string md5 = Cryptography.MD5Hash(get);
                        //Other obj = Database.FindRaw(md5);
                        Other obj = Database.FindRaw(666);
                        Console.WriteLine("Выполняется поиск...");
                        Console.WriteLine("Поиск не дал результатов.");
                        //handler.Send(Encoding.Unicode.GetBytes(t.ToString()));
                        
                        if (obj != null)
                        {   
                            Bitmap bmp = new Bitmap(Image.FromStream(new MemoryStream(get)));                            
                            string info = Encoding.Unicode.GetString(Cryptography.Pull(bmp, bmp.Width, bmp.Height, 304));
                            string key = Operations.GetEncode(obj.Control);
                            byte[] result = Cryptography.Gamma(key, info);
                            handler.Send(result);
                            bmp.Dispose();
                        }
                        else
                        {
                            handler.Send(Encoding.Unicode.GetBytes("_Negative_"));
                        }
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
    }
}
