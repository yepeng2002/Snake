using Snake.Core.Enums;
using Snake.Core.Ioc;
using Snake.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Snake.Api.Controllers
{
    public class BaseApiController : ApiController
    {
        #region 1. Ioc Container
        [NonAction]
        public object GetService(string servicename)
        {
            return IocContainer.GetService(servicename);
        }

        [NonAction]
        public object GetService(Type t)
        {
            return IocContainer.GetService(t);
        }

        [NonAction]
        public T GetService<T>()
        {
            return IocContainer.GetService<T>();
        }
        #endregion

        #region 2. IHttpActionResult

        [NonAction]
        public TransData<T> Response<T>(T data)
        {

            var result = new TransData<T>()
            {
                Data = data,
                Message = "操作成功",
                Code = (int)ServiceResultCode.Succeeded
            };

            return result;
        }

        [NonAction]
        public TransData<T> Response<T>(string message, int code)
        {

            var result = new TransData<T>()
            {
                Data = default(T),
                Message = message,
                Code = code
            };

            return result;
        }

        [NonAction]
        public TransData<T> Response<T>(T data, string message)
        {
            var result = new TransData<T>()
            {
                Data = data,
                Message = message,
                Code = (int)ServiceResultCode.Succeeded
            };

            return result;
        }

        [NonAction]
        public TransData<T> Response<T>(T data, string message, int code)
        {
            var result = new TransData<T>()
            {
                Data = data,
                Message = message,
                Code = code
            };

            return result;
        }

        #endregion
    }
}