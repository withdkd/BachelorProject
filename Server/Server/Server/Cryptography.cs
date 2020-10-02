using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Drawing;
using System.Collections;

namespace Server
{
    internal static class Cryptography
    {
        internal static readonly Int64 MAX = 274877906943;

        //Внедрение информации внутрь изображения
        internal static byte[] Insert(Bitmap src, Other obj, int width, int height)
        {
            //Байты зашифрованной строки            
            string key = GenerateKey();
            obj.Date = DateTime.Now;
            string info = obj.Date.ToShortDateString();
            byte[] text = Gamma(key, info);
            string str = "";
            for (int i = 0; i < text.Length; i++)
            {
                BitArray bit = Operations.ByteToBit(text[i]);
                foreach (bool b in bit)
                {
                    if (b)
                        str += "1";
                    else
                        str += "0";
                }
            }
            int index = 0;
            BitArray color;            
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color pixel = src.GetPixel(i, j);
                    byte newR = pixel.R;
                    byte newG = pixel.G;
                    byte newB = pixel.B;
                    if (index != str.Length - 1)
                    {
                        color = Operations.ByteToBit(pixel.R);
                        if (str[index] == '1')
                            color[0] = true;
                        else
                            color[0] = false;
                        index++;
                        newR = Operations.BitToByte(color);
                    }
                    if (index != str.Length - 1)
                    {
                        color = Operations.ByteToBit(pixel.G);
                        if (str[index] == '1')
                            color[0] = true;
                        else
                            color[0] = false;
                        index++;
                        newG = Operations.BitToByte(color);
                    }
                    if (index != str.Length - 1)
                    {
                        color = Operations.ByteToBit(pixel.B);
                        if (str[index] == '1')
                            color[0] = true;
                        else
                            color[0] = false;
                        index++;
                        newG = Operations.BitToByte(color);
                    }
                    Color newColor = Color.FromArgb(newR, newG, newB);
                    src.SetPixel(i, j, newColor);
                    if (index == str.Length - 1)
                        break;
                }
            }            
            byte[] answer = (byte[])new ImageConverter().ConvertTo(src, typeof(byte[]));
            string md5 = MD5Hash(answer);
            obj.Md5 = md5;
            obj.Control = Operations.SetEncode(key);
            Console.WriteLine("old: {0}", obj.Date.ToString());
            Database.Insert(obj);
            return answer;
        }

        //Шифрование
        internal static byte[] Gamma(string key, string info)
        {
            string result = "";
            for (int i = 0; i < info.Length; i++)
            {
                byte up = (byte)key[i];
                byte down = (byte)info[i];
                result += (up ^ down).ToString();
            }
            return Encoding.Unicode.GetBytes(result);
        }

        //Изъятие информации из изображения
        internal static byte[] Pull(Bitmap src, int width, int height, int len)
        {
            string text = "";
            string answer = "";
            BitArray color;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color pixel = src.GetPixel(i, j);
                    if (len != -1)
                    {
                        color = Operations.ByteToBit(pixel.R);
                        if (color[0])
                            text += "1";
                        else
                            text += "0";
                        len--;
                    }
                    if (len != -1)
                    {
                        color = Operations.ByteToBit(pixel.G);
                        if (color[0])
                            text += "1";
                        else
                            text += "0";
                        len--;
                    }
                    if (len != -1)
                    {
                        color = Operations.ByteToBit(pixel.B);
                        if (color[0])
                            text += "1";
                        else
                            text += "0";
                        len--;
                    }
                    if (len == -1)
                        break;
                }
            }
            if (text.Length % 8 != 0)
            {
                int count = (text.Length / 8 + 1) * 8 - text.Length;
                for (int i = 0; i < count; i++)
                    text += "0";
            }
            List<BitArray> array = new List<BitArray>();
            for (int i = 0; i < text.Length - 1;)
            {
                BitArray bit = new BitArray(8);
                string sub = text.Substring(i, i + 7);
                for (int j = 0; j < 7; j++)
                {
                    if (sub[j] == '1')
                    {
                        bit[j] = true;
                    }
                    else
                    {
                        bit[j] = false;
                    }
                }
                i += 8;
                array.Add(bit);
            }
            for (int i = 0; i < array.Count; i++)
            {
                answer += Operations.BitToByte(array[i]).ToString();
            }
            return Encoding.Unicode.GetBytes(answer);
        }
        //Генерация секретного ключа для шифрования

        internal static string GenerateKey()
        {
            Random rnd = new Random();
            Int64[] number = new Int64[8];
            string result = "";
            for (int j = 0; j < 8; j++)
            {
                double rand = rnd.NextDouble();
                string reverse = rand.ToString();
                string a = "";
                for (int i = reverse.Length - 1; i >= 0; i--)
                    a += reverse[i];
                a = a.Substring(0, a.Length - 6);
                number[j] = Int64.Parse(a);
                if (number[j] > MAX)
                    number[j] = number[j] % MAX;
                string bitString = Convert.ToString(number[j], 2);
                if (bitString.Length != 38)
                {
                    int l = 38 - bitString.Length;
                    for (int k = 0; k < l; k++)
                        bitString = "0" + bitString;
                }
                result += bitString;
            }
            return result;
        }
        //Создание MD5 хэша
        internal static string MD5Hash(byte[] src)
        {
            string result = "";
            using (MD5 md5 = MD5.Create())
            {                
                byte[] hash = md5.ComputeHash(src);
                for (int i = 0; i < hash.Length; i++)
                {
                    if (hash[i].ToString() == "-")
                        result += string.Empty;
                    else
                        result += hash[i].ToString();
                }
            }
            return result;
        }
    }
}

