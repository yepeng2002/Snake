using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Core.Models
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAppLog
    {
        string Guid { get; set; }

        DateTime CTime { get; set; }
        
        string Application { get; set; }
        
        string AppPath { get; set; }

        string IPv4 { get; set; }

        string Machine { get; set; }

        string LogCategory { get; set; }

        string Message { get; set; }
        
        int Level { get; set; }
        IList<string> Tags { get; set; }
    }
}
