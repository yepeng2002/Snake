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
        public object GetService(string servicename)
        {
            return IocContainer.GetService(servicename);
        }

        public object GetService(Type t)
        {
            return IocContainer.GetService(t);
        }

        public T GetService<T>()
        {
            return IocContainer.GetService<T>();
        }
    }
}