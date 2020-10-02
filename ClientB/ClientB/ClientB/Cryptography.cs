using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ClientB
{
    internal static class Cryptography
    {
        internal static string GetMD5(byte[] src)
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
