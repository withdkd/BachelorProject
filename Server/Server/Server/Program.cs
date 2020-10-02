using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using LiteDB;
using System.IO;

namespace Server
{
    internal static class Program
    {               
        internal static void Main(string[] args)
        {   
            if (File.Exists(Directory.GetCurrentDirectory() + @"/ip.ini"))
            {
                using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + @"/ip.ini"))
                {
                    Network.Ip = sr.ReadToEnd();
                }
                Thread thread1 = new Thread(Network.ServerForClients);
                Thread thread2 = new Thread(Network.ServerForCheckers);
                thread1.Start();
                thread2.Start();
            }
            else
            {
                Console.WriteLine("ip.ini не существует или не содержит адреса сервера.");
            }
            Console.ReadKey();
        }   
        //debug
        internal static void Testc()
        {
            
        }
        //debug
        internal static Other InsertRandomRaws()
        {
            Random rnd = new Random();
            Other obj = new Other
            {
                Date = DateTime.Now,
                Width = rnd.Next(),
                Height = rnd.Next(),
                Md5 = rnd.Next().ToString(),
                Control = new string[48]
            };
            return obj;
        }
        //debug
        internal static void CreateDB()
        {
            using (var db = new LiteDatabase(@"repository0.db"))
            {
                var coll = db.GetCollection<Other>("table");
                Other o = new Other
                {
                    Width = 666,
                    Height = 666,
                    Md5 = "",
                    Date = DateTime.Now,
                    Control = new string[48]
                };
                coll.Insert(o);
                for (int i = 0; i < 100; i++)
                {
                    Other obj = InsertRandomRaws();
                    coll.Insert(obj);                    
                }
            }
        }
        //debug
        internal static void Search()
        {
            using (var db = new LiteDatabase(@"repository0.db"))
            {
                var coll = db.GetCollection<Other>("table");
                var all = coll.FindOne(x => x.Width == 666);
                Console.Write(all.Date);                
            }
        }
    }
}
