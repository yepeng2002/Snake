using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Core.Enums
{
    #region LogCategory
    /// <summary>
    /// 日志分类
    /// </summary>
    public enum LogCategory
    {
        /// <summary>
        /// Debug category.
        /// </summary>
        Debug,

        /// <summary>
        /// Info category.
        /// </summary>
        Info,

        /// <summary>
        /// Warn category.
        /// </summary>
        Warn,

        /// <summary>
        /// Error category.
        /// </summary>
        Error,

        /// <summary>
        /// Fatal category.
        /// </summary>
        Fatal
    }
    #endregion
}
