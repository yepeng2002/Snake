using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Client.WebApi
{
    public class HttpRequestResult
    {
        public int Code { get; set; }

        public string Data { get; set; }

        public string Message { get; set; }
    }
}
