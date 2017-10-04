using Snake.Api.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Snake.Api.Models
{
    public class TransData<T>
    {
        public int state { get; set; }
        public string message { get; set; }
        public T Data { get; set; }

        public TransData()
        {
            state = (int)ServiceResultCode.Succeeded;
        }
    }
}