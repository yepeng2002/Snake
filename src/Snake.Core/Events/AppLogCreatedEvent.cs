using MongoDB.Bson.Serialization.Attributes;
using Snake.Core.Models;
using Snake.Core.Mongo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Core.Events
{
    /// <summary>
    /// 新增应用日志时间实体类
    /// </summary>
    public class AppLogCreatedEvent : BaseEvent, IAppLog
    {
        public string Guid { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        private DateTime _cTime;
        /// <summary>
        /// 日志生成时间
        /// </summary>
        public DateTime CTime
        {
            get { return _cTime; }
            set
            {
                if (value == _cTime)
                {
                    return;
                }
                _cTime = value;
                this.NotifyPropertyChanged(() => this.CTime);
            }
        }

        private string _application;
        /// <summary>
        /// 应用标识
        /// </summary>
        [Required, MaxLength(50)]
        public string Application
        {
            get { return _application; }
            set
            {
                if (value == _application)
                {
                    return;
                }
                _application = value;
                this.NotifyPropertyChanged(() => this.Application);
            }
        }

        private string _appPath;
        /// <summary>
        /// 应用路径
        /// </summary>
        public string AppPath
        {
            get { return _appPath; }
            set
            {
                if (value == _appPath)
                {
                    return;
                }
                _appPath = value;
                this.NotifyPropertyChanged(() => this.AppPath);
            }
        }

        private string _iPv4;
        /// <summary>
        /// 内网IP
        /// </summary>
        public string IPv4
        {
            get { return _iPv4; }
            set
            {
                if (value == _iPv4)
                {
                    return;
                }
                _iPv4 = value;
                this.NotifyPropertyChanged(() => this.IPv4);
            }
        }

        private string _machine;
        /// <summary>
        /// 机器名
        /// </summary>
        public string Machine
        {
            get { return _machine; }
            set
            {
                if (value == _machine)
                {
                    return;
                }
                _machine = value;
                this.NotifyPropertyChanged(() => this.Machine);
            }
        }

        private string _logCategory;
        /// <summary>
        /// 日志类别: Debug, Info, Warn, Error, Fatal
        /// </summary>
        public string LogCategory
        {
            get { return _logCategory; }
            set
            {
                if (value == _logCategory)
                {
                    return;
                }
                _logCategory = value;
                this.NotifyPropertyChanged(() => this.LogCategory);
            }
        }

        private string _message;
        /// <summary>
        /// 日志消息内容
        /// </summary>
        public string Message
        {
            get { return _message; }
            set
            {
                if (value == _message)
                {
                    return;
                }
                _message = value;
                this.NotifyPropertyChanged(() => this.Message);
            }
        }

        private int _level;
        /// <summary>
        /// 级别：1,2,3,4,5
        /// 级别越小越重要
        /// 1：发送短信提醒
        /// 2：发送邮件提醒
        /// 3,4,5：无
        /// </summary>
        public int Level
        {
            get { return _level; }
            set
            {
                if (value == _level)
                {
                    return;
                }
                _level = value;
                this.NotifyPropertyChanged(() => this.Level);
            }
        }

        private IList<string> _tags;
        /// <summary>
        /// 标签
        /// </summary>
        public IList<string> Tags
        {
            get { return _tags; }
            set
            {
                if (value == _tags)
                {
                    return;
                }
                _tags = value;
                this.NotifyPropertyChanged(() => this.Tags);
            }
        }
    }
}
