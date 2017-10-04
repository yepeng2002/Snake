using Snake.Core.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Snake.Api.Services
{
    public class BaseService
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