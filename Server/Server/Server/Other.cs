using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace Server
{
    public class Other
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Md5 { get; set; }
        public DateTime Date { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string[] Control { get; set; }

        public Other()
        {
            Id = Guid.NewGuid();
        }        
    }
}
