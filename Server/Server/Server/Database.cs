using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace Server
{
    internal static class Database
    {        
        internal static void Insert(Other obj)
        {
            try
            {
                using (LiteDatabase db = new LiteDatabase(@"Data.db"))
                {
                    LiteCollection<Other> collection = db.GetCollection<Other>("data");
                    collection.Insert(obj);
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }            
        }

        internal static Other FindRaw(int w)
        {
            Other obj = null;
            try
            {
                using (LiteDatabase db = new LiteDatabase(@"Data.db"))
                {
                    LiteCollection<Other> collection = db.GetCollection<Other>("data");
                    obj = collection.FindOne(x => x.Width == 666);
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
            return obj;
        }
        internal static Other FindRaw(string md5)
        {
            Other obj = null;
            try
            {
                using (LiteDatabase db = new LiteDatabase(@"Data.db"))
                {
                    LiteCollection<Other> collection = db.GetCollection<Other>("data");
                    obj = collection.FindOne(x => x.Md5 == md5);
                    if (obj != null)
                        return obj;                    
                }                
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
            return null;
        }
    }
}
