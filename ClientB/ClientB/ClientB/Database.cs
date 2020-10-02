using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace ClientB
{
    internal static class Database
    {
        internal static void InsertIntoDb(Other obj)
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
        internal static Other PullFromDb(DateTime date)
        {
            Other obj = null;
            try
            {
                using (LiteDatabase db = new LiteDatabase(@"Data.db"))
                {
                    LiteCollection<Other> collection = db.GetCollection<Other>("data");
                    obj = collection.FindOne(x => x.date == date);
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
