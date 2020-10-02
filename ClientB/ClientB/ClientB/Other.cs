using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace ClientB
{
    public class Other
    {
        [BsonId]
        public Guid id { get; set; }
        public DateTime date { get; set; }
        public string md5 { get; set; }
        public string result { get; set; }
        public Other()
        {
            id = Guid.NewGuid();
        }
    }
}
