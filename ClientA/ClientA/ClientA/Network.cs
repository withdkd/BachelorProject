using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace ClientA
{
    internal static class Network
    {
        internal static string Ip_server { get; set; }
        internal static string Ip_judge { get; set; } 
        internal static int Port_server { get; } = 50000;
        internal static int Port_judge { get; } = 50002;

        internal static void SendInfo(byte[] src)
        {
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(Ip_server), Port_server);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipPoint);
                socket.Send(src);
                byte[] data = new byte[256];

                List<byte> vs = new List<byte>();
                int bytes = 0;
                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    vs.AddRange(data);
                }
                while (socket.Available > 0);
                if (vs.Count > 0)
                {
                    byte[] receive = new byte[vs.Count];
                    for (int i = 0; i < vs.Count; i++)
                        receive[i] = vs[i];
                    File.Delete(Directory.GetCurrentDirectory() + @"/image1_1.bmp");
                    Data.SetImage(receive);
                }
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }

        internal static void SendToCheck(byte[] src)
        {
            try
            {
                IPEndPoint point = new IPEndPoint(IPAddress.Parse(Ip_judge), Port_judge);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(point);
                socket.Send(src);
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }
    }
}
