using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace ClientA
{
    internal static class Data
    {
        internal static int Width { get; set; }
        internal static int Height { get; set; }        

        private static AppDomain domain = AppDomain.CurrentDomain;

        internal static byte[] ReSaveImage(byte[] src, string name)
        {
            MemoryStream ms = new MemoryStream(src);
            Image img = Image.FromStream(ms);
            Bitmap bmp = new Bitmap(img);
            ms.Close();
            name = "_" + name;            
            bmp.Save(name, System.Drawing.Imaging.ImageFormat.Bmp);
            return File.ReadAllBytes(name);
        }
        internal static void DeleteTmp()
        {
            if (Directory.Exists(Directory.GetCurrentDirectory() + "/temp/"))
            {
                string[] files = Directory.GetFiles(Directory.GetCurrentDirectory() + "/temp/");
                foreach (string s in files)
                {
                    File.Delete(s);
                }
            }
            Directory.Delete(Directory.GetCurrentDirectory() + @"\temp\");
        }
        internal static byte[] SelectStruct(byte[] src)
        {
            if (Directory.Exists(Directory.GetCurrentDirectory() + "/temp/"))
            {
                string[] files = Directory.GetFiles(Directory.GetCurrentDirectory() + "/temp/");
                foreach (string s in files)
                {
                    File.Delete(s);
                }
                Directory.Delete(Directory.GetCurrentDirectory() + @"/temp/");
            }
            Directory.CreateDirectory(domain.BaseDirectory + "\\temp\\");
            Image img = Image.FromStream(new MemoryStream(src));
            Bitmap bmp;
            Graphics g;
            int h = img.Height;
            int w = img.Width;
            Width = w;
            Height = h;
            int nx = 4;
            int ny = 4;
            int[] x = new int[nx + 1];
            int[] y = new int[ny + 1];
            x[0] = 0;
            y[0] = 0;
            for (int i = 1; i <= nx; i++)
            {
                x[i] = w * i / nx;                
            }
            for (int i = 1; i <= ny; i++)
            {
                y[i] = h * i / ny;
            }
            for (int i = 0; i < nx; i++)
            {
                for (int j = 0; j < ny; j++)
                {
                    w = x[i + 1] - x[i];
                    h = y[j + 1] - y[j];
                    bmp = new Bitmap(w, h);
                    g = Graphics.FromImage(bmp);
                    g.DrawImage(img, new Rectangle(0, 0, w, h), new Rectangle(x[i], y[j], w, h), GraphicsUnit.Pixel);
                    //bmp.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("image{0}_{1}.bmp", i, j)), System.Drawing.Imaging.ImageFormat.Bmp);
                    string path = domain.BaseDirectory + string.Format("\\temp\\image{0}_{1}.bmp", i, j);
                    bmp.Save(path, System.Drawing.Imaging.ImageFormat.Bmp);
                    g.Dispose();
                    bmp.Dispose();
                }
            }
            img.Dispose();
            return File.ReadAllBytes(Directory.GetCurrentDirectory() + @"/temp/image1_1.bmp");            
        }
        internal static void SetImage(byte[] src)
        {
            MemoryStream ms = new MemoryStream(src);
            Image img = Image.FromStream(ms);
            Bitmap bmp = new Bitmap(img);
            ms.Close();
            //File.Delete(Directory.GetCurrentDirectory() + "\\temp\\image1_1.bmp");
            bmp.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("\\temp\\image1_1.bmp")), System.Drawing.Imaging.ImageFormat.Bmp);
            bmp.Dispose();
        }
        internal static void SetFullImage()
        {
            try
            {
                Bitmap full = new Bitmap(Width, Height);
                Graphics graphics = Graphics.FromImage(full);
                Image fragment;
                using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + @"\log.txt"))
                {
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            fragment = Image.FromFile(string.Format(Directory.GetCurrentDirectory() + @"\temp\image{0}_{1}.bmp", i, j));
                            sw.WriteLine(string.Format("image{0}_{1}.bmp", i, j));
                            graphics.DrawImage(fragment, i * Width / 4, j * Height / 4);
                            fragment.Dispose();
                        }
                    }
                }
                full.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("fullImage.bmp")), System.Drawing.Imaging.ImageFormat.Bmp);                
                full.Dispose();
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }
        internal static byte[] SelectFragmentForCheck(byte[] src)
        {           
            try
            {
                MemoryStream ms = new MemoryStream(src);
                Image img = Image.FromStream(ms);
                Graphics g;
                Bitmap bmp;
                int h = img.Height;
                int w = img.Width;
                Width = w;
                Height = h;
                int nx = 4;
                int ny = 4;
                int[] x = new int[nx + 1];
                int[] y = new int[ny + 1];
                x[0] = 0;
                y[0] = 0;
                for (int i = 1; i <= nx; i++)
                {
                    x[i] = w * i / nx;
                }
                for (int i = 1; i <= ny; i++)
                {
                    y[i] = h * i / ny;
                }
                for (int i = 0; i < nx; i++)
                {
                    for (int j = 0; j < ny; j++)
                    {
                        w = x[i + 1] - x[i];
                        h = y[j + 1] - y[j];
                        bmp = new Bitmap(w, h);
                        g = Graphics.FromImage(bmp);
                        g.DrawImage(img, new Rectangle(0, 0, w, h), new Rectangle(x[i], y[j], w, h), GraphicsUnit.Pixel);                        
                        string path = domain.BaseDirectory + string.Format("image{0}_{1}.bmp", i, j);
                        bmp.Save(path, System.Drawing.Imaging.ImageFormat.Bmp);
                        g.Dispose();
                        bmp.Dispose();
                    }
                }
                img.Dispose();
                ms.Close();
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
            return File.ReadAllBytes(Directory.GetCurrentDirectory() + @"/image1_1.bmp");
        }
    }
}
