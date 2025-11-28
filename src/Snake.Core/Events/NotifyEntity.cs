using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Snake.Core.Events
{
    /// <summary>
    /// Notify基类
    /// </summary>
    public class NotifyEntity : INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 基于属性名的实现
        /// </summary>
        /// <param name="propertyName"></param>
        public virtual void NotifyPropertyChanged(String propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// 基于Lambda实现
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        public void NotifyPropertyChanged<T>(Expression<Func<T>> propertyName)
        {
            if (this.PropertyChanged != null)
            {
                var memberExpression = propertyName.Body as MemberExpression;
                if (memberExpression != null)
                {
                    NotifyPropertyChanged(memberExpression.Member.Name);
                }
            }
        }
    }
}
