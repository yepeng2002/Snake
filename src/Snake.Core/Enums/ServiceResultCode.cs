using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Snake.Core.Enums
{
    /// <summary>
    /// 服务返回结果状态编号
    /// 1：成功；2：无数据；3：参数校验错误；255：其他错误；
    /// </summary>
    public enum ServiceResultCode
    {
        Succeeded = 1,
        NoData = 2,
        ParameterError = 3,
        UndefinedError = 255
    }

}