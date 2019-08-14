using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Core.Mongo
{
    public class QueryParams
    {
        public QueryParams()
        {
            Index = 1;
            Size = 15;
        }
        
        public int Total { get; set; }
        
        public int Index { get; set; }
        
        public int Size { get; set; }

    }
}
