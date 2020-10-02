using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Server
{
    internal static class Operations
    {
        internal readonly static Dictionary<string, string> control = new Dictionary<string, string>()
        {
            { "0000", "0x0" },
            { "0001", "0x1" },
            { "0010", "0x2" },
            { "0011", "0x3" },
            { "0100", "0x4" },
            { "0101", "0x5" },
            { "0110", "0x6" },
            { "0111", "0x7" },
            { "1000", "0x8" },
            { "1001", "0x9" },
            { "1010", "0xA" },
            { "1011", "0xB" },
            { "1100", "0xC" },
            { "1101", "0xD" },
            { "1110", "0xE" },
            { "1111", "0xF" }
        };
        internal static BitArray ByteToBit(byte src)
        {
            BitArray bitArray = new BitArray(8);
            bool state = false;
            for (int i = 0; i < 8; i++)
            {
                if ((src >> i & 1) == 1)
                    state = true;
                else
                    state = false;
                bitArray[i] = state;
            }
            return bitArray;
        }
        internal static byte BitToByte(BitArray src)
        {
            byte number = 0;
            for (int i = 0; i < src.Count; i++)
                if (src[i] == true)
                    number += (byte)Math.Pow(2, i);
            return number;
        }
        internal static string[] SetEncode(string value)
        {
            string[] controls = new string[76];
            int j = 0;
            for (int i = 0; i < value.Length; i += 4)
            {
                string a = string.Concat(value[i], value[i + 1], value[i + 2], value[i + 3]);
                foreach (KeyValuePair<string, string> pair in control)
                {
                    if (a == pair.Key)
                    {
                        controls[j] = pair.Value;
                        break;
                    }
                }
                j++;
                //Console.WriteLine(controls[j].ToString());
            }            
            return controls;
        }
        internal static string GetEncode(string[] values)
        {
            string result = "";
            for (int i = 0; i < values.Length; i++)
            {
                foreach (KeyValuePair<string, string> pair in control)
                {
                    if (values[i] == pair.Value)
                    {
                        result += pair.Key;
                        break;
                    }
                }
            }
            Console.WriteLine("Control string: {0}", result);
            return result;
        }        
    }
}
