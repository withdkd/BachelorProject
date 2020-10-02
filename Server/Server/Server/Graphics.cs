using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace Server
{
    internal static class Graphics
    {
        internal static Bitmap SetImage(Other obj, byte[] src)
        {
            MemoryStream ms = new MemoryStream(src);
            Bitmap bmp = new Bitmap(Image.FromStream(ms));
            obj.Width = bmp.Width;
            obj.Height = bmp.Height;
            ms.Close();
            ms.Dispose();
            return bmp;
        }
    }
}
