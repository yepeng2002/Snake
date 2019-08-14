using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Core.Ioc
{
    public class IocContainer
    {
        private static WindsorContainer _container;
        public static void SetContainer(WindsorContainer container)
        {
            _container = container;
        }
        public static WindsorContainer GetContainer()
        {
            if (_container == null)
            {
                _container = new WindsorContainer();
            }
            return _container;
        }

        public static void CloseContex()
        {
            GetContainer().Dispose();
        }

        /// <summary>
        /// 根据服务名得到服务
        /// <remarks>
        /// </remarks>
        /// </summary>
        /// <param name="servicename"></param>
        /// <returns></returns>
        public static object GetService(string servicename)
        {
            try
            {
                object o = GetContainer()[servicename];
                if (o != null)
                {
                    return o;
                }
                else
                {
                    throw new Exception("WindsorContainer获取服务异常！");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据类型得到服务
        /// <remarks>
        /// 
        /// </remarks>
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static object GetService(Type t)
        {
            try
            {
                object o = GetContainer().Resolve(t);
                if (o != null)
                {
                    return o;
                }
                else
                {
                    throw new Exception("WindsorContainer获取服务异常！");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据类型得到服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetService<T>()
        {

            try
            {
                object o = GetContainer().Resolve<T>();
                if (o != null)
                {
                    return (T)o;
                }
                else
                {
                    throw new Exception("WindsorContainer获取服务异常！");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 释放对象
        /// </summary>
        /// <param name="o"></param>
        public static void Release(object o)
        {
            GetContainer().Release(o);
        }

    }
}
