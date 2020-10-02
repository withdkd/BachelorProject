using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Server
{
    public static class Debug
    {
        public static void Log(Exception ex)
        {
            try
            {
                if (!Directory.Exists(Directory.GetCurrentDirectory() + @"/Logs"))
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"/Logs");
                using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + @"/Logs/Errorslog.txt"))
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
    }
}
