using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ClientB
{
    public static class Debug
    {
        public static void Log(Exception ex)
        {
            try
            {
                if (!Directory.Exists(Directory.GetCurrentDirectory() + @"/Log"))
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"/Log");
                using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + @"/Log/ErrorsLog.txt"))
                {
                    sw.WriteLine("[" + DateTime.Now.ToString() + "] \nMessage: " + ex.Message + "\nSource: \n" + ex.Source + "\nStackTrace: \n" + ex.StackTrace);
                }
                
            }
            catch (Exception)
            { }
            finally
            {
                throw new Exception();
            }
        }
        public static void Log(string info)
        {
            try
            {
                if (!Directory.Exists(Directory.GetCurrentDirectory() + @"/Log"))
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"/Log");
                using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + @"/Log/ErrorsLog.txt"))
                {
                    sw.WriteLine("[" + DateTime.Now.ToString() + "] " + info);
                }
                
            }
            catch (Exception)
            { }
            finally { throw new Exception(); }
        }
    }
}
