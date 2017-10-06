using Snake.Core.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Snake.Api.Controllers
{
    public class BaseApiController : ApiController
    {
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
    }
}