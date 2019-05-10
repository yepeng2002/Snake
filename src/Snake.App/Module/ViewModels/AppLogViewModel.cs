using Snake.App.Controls.Mvvm;
using Snake.App.Module.Models;
using Snake.Client.WebApi;
using Snake.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Messenger = Snake.App.Controls.Mvvm.Messenger;

namespace Snake.App.Module.ViewModels
{
    public class AppLogViewModel : Controls.Mvvm.ViewModelBase
    {
        public AppLogViewModel()
        {
            //Set Command
            OnViewLoadedCommand = new DelegateCommand(this.OnViewLoadedCommandExecute);
            OnQueryCommand = new DelegateCommand(this.OnQueryCommandExecute);
            OnQueryNextPageCommand = new DelegateCommand(this.OnQueryNextPageCommandExecute);
            OnMessageSortDescCommand = new DelegateCommand(this.OnMessageSortDescCommandExecute);
            OnMessageSortAscCommand = new DelegateCommand(this.OnMessageSortAscCommandExecute);

            //
            LogCategorys = new ObservableCollection<string>() { "Debug", "Info", "Warn", "Error", "Fatal" };
            Levels = new ObservableCollection<int>() { 1, 2, 3, 4, 5 };

            this.PageAppLog = new PageAppLog()
            {
                PageIndex = 1,
                PageSize = 800,
                CTime = DateTime.Now.AddHours(-4),
                CTimeEnd = DateTime.Now
            };
        }

        #region private method

        private async void QueryAppLogsAsync()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            SnakeWebApiHttpProxy proxy = new SnakeWebApiHttpProxy();
            var result = await proxy.GetAppLogsPageAsync<IList<AppLog>>(this.PageAppLog);
            stopWatch.Stop();
            if (result.Item1 && result.Item2 != null)
                AppLogs = new ObservableCollection<AppLog>(result.Item2);
            else
                AppLogs = null;
            string log = string.Format("AppLog数据：{0}条  查询用时：{1} ms.", AppLogs == null ? 0 : AppLogs.Count, stopWatch.Elapsed.TotalMilliseconds.ToString("0."));
            //DevExpress.Xpf.Mvvm.Messenger.Default.Send(new StatusUpdateMessage(log));
            Messenger.Default.Send(new StatusUpdateMessage(log));
        }

        #endregion


        #region command
        /// <summary>
        /// 视图首次加载
        /// </summary>
        public ICommand OnViewLoadedCommand { get; private set; }
        public ICommand OnQueryCommand { get; private set; }
        public ICommand OnQueryNextPageCommand { get; private set; }
        public ICommand OnMessageSortDescCommand { get; private set; }
        public ICommand OnMessageSortAscCommand { get; private set; }

        private async void OnViewLoadedCommandExecute()
        {
            if (IsFirstLoad)
            {
                Status = ViewModelStatus.Loading;
                try
                {
                    QueryAppLogsAsync();
                }
                finally
                {
                    Status = ViewModelStatus.Loaded;
                    IsFirstLoad = false;
                }
            }
        }
        
        private async void OnQueryCommandExecute()
        {
            Status = ViewModelStatus.Loading;
            try
            {
                QueryAppLogsAsync();
            }
            finally
            {
                Status = ViewModelStatus.Loaded;
            }
        }

        private async void OnQueryNextPageCommandExecute()
        {
            Status = ViewModelStatus.Loading;
            try
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                SnakeWebApiHttpProxy proxy = new SnakeWebApiHttpProxy();
                var result = await proxy.GetAppLogsPageAsync<IList<AppLog>>(this.PageAppLog);
                stopWatch.Stop();
                if (result.Item1 && result.Item2 != null)
                {
                    var list = AppLogs.Union<AppLog>(result.Item2).ToList<AppLog>();
                    AppLogs = new ObservableCollection<AppLog>(list);
                }
                string log = string.Format("AppLog数据：{0}条  用时：{1} ms.", AppLogs == null ? 0 : AppLogs.Count, stopWatch.Elapsed.TotalMilliseconds.ToString("0."));
                //DevExpress.Xpf.Mvvm.Messenger.Default.Send(new StatusUpdateMessage(log));
                Messenger.Default.Send(new StatusUpdateMessage(log));
            }
            finally
            {
                Status = ViewModelStatus.Loaded;
            }
        }

        private async void OnApplicationsDropDownOpenedCommandExecute()
        {
            var dto = new QueryParamDto() { StrParam = "" };
            SnakeWebApiHttpProxy proxy = new SnakeWebApiHttpProxy();
            var result = await proxy.QueryApplicationOfAppLogAsync<IList<string>>(dto);
            if (result.Item1 && result.Item2 != null)
            {
                Applications = new ObservableCollection<string>(result.Item2);
            }
        }
        
        private async void OnTagIsDropDownOpenCommandExecute()
        {
            var dto = new QueryParamDto() { StrParam = "" };
            SnakeWebApiHttpProxy proxy = new SnakeWebApiHttpProxy();
            var result = await proxy.QueryTagsOfAppLogAsync<IList<string>>(dto);
            if (result.Item1 && result.Item2 != null)
            {
                Tags = new ObservableCollection<string>(result.Item2);
            }
        }

        /// <summary>
        /// 异常消息文本逆序排列
        /// </summary>
        private void OnMessageSortDescCommandExecute()
        {
            if (SelectedApplogs != null && SelectedApplogs.Count > 0)
            {
                var list = new List<string>();
                foreach (var item in SelectedApplogs)
                {
                    list.Add((item as AppLog).Message);
                }
                Messages = string.Join("\r\n", list.ToArray());
            }
            else
                Messages = string.Empty;
        }
        /// <summary>
        /// 异常消息文本升序排列
        /// </summary>
        private void OnMessageSortAscCommandExecute()
        {
            if (SelectedApplogs != null && SelectedApplogs.Count > 0)
            {
                int count = SelectedApplogs.Count;
                var list = new List<string>();
                for (int i = SelectedApplogs.Count - 1; i >= 0; i--)
                {
                    list.Add((SelectedApplogs[i] as AppLog).Message);
                }
                Messages = string.Join("\r\n", list.ToArray());
            }
            else
                Messages = string.Empty;
        }
        #endregion


        #region  属性

        private PageAppLog _pageAppLog;
        public PageAppLog PageAppLog
        {
            get { return _pageAppLog; }
            set
            {
                if (value == _pageAppLog)
                {
                    return;
                }
                _pageAppLog = value;
                RaisePropertyChanged(() => PageAppLog);
            }
        }

        private ObservableCollection<string> _logCategorys;
        public ObservableCollection<string> LogCategorys
        {
            get { return _logCategorys; }
            set
            {
                if (value == _logCategorys)
                {
                    return;
                }
                _logCategorys = value;
                RaisePropertyChanged(() => LogCategorys);
            }
        }

        private ObservableCollection<int> _levels;
        public ObservableCollection<int> Levels
        {
            get { return _levels; }
            set
            {
                if (value == _levels)
                {
                    return;
                }
                _levels = value;
                RaisePropertyChanged(() => Levels);
            }
        }

        private ObservableCollection<string> _applications;
        public ObservableCollection<string> Applications
        {
            get { return _applications; }
            set
            {
                if (value == _applications)
                {
                    return;
                }
                _applications = value;
                RaisePropertyChanged(() => Applications);
            }
        }

        private ObservableCollection<string> _tags;
        public ObservableCollection<string> Tags
        {
            get { return _tags; }
            set
            {
                if (value == _tags)
                {
                    return;
                }
                _tags = value;
                RaisePropertyChanged(() => Tags);
            }
        }

        private ObservableCollection<AppLog> _appLogs;
        public ObservableCollection<AppLog> AppLogs
        {
            get { return _appLogs; }
            set
            {
                if (value == _appLogs)
                {
                    return;
                }
                _appLogs = value;
                RaisePropertyChanged(() => AppLogs);
            }
        }

        private AppLog _selectedApplog;
        public AppLog SelectedApplog
        {
            get { return _selectedApplog; }
            set
            {
                if (value == _selectedApplog)
                {
                    return;
                }
                _selectedApplog = value;
                RaisePropertyChanged(() => SelectedApplog);
                if (_selectedApplog != null)
                    Messages = _selectedApplog.Message;
                else
                    Messages = string.Empty;
            }
        }

        private IList _selectedApplogs;
        public IList SelectedApplogs
        {
            get { return _selectedApplogs; }
            set
            {
                _selectedApplogs = value;
                RaisePropertyChanged(() => SelectedApplogs);
                if (_selectedApplogs != null && _selectedApplogs.Count > 0)
                {
                    var list = new List<string>();
                    foreach (var item in _selectedApplogs)
                    {
                        list.Add((item as AppLog).Message);
                    }
                    Messages = string.Join("\r\n", list.ToArray());
                }
                else
                    Messages = string.Empty;
            }
        }

        private int? _level;
        public int? Level
        {
            get { return _level; }
            set
            {
                if (value == _level)
                {
                    return;
                }
                _level = value;
                RaisePropertyChanged(() => Level);
                if (PageAppLog != null)
                    if (_level != null)
                        PageAppLog.Level = _level.Value;
                    else
                        PageAppLog.Level = 0;
            }
        }

        private string _messages;
        public string Messages
        {
            get { return _messages; }
            set
            {
                if (value == _messages)
                {
                    return;
                }
                _messages = value;
                RaisePropertyChanged(() => Messages);
            }
        }

        private bool _applicationIsDropDownOpen;
        public bool ApplicationIsDropDownOpen
        {
            get { return _applicationIsDropDownOpen; }
            set
            {
                if (value == _applicationIsDropDownOpen)
                {
                    return;
                }
                _applicationIsDropDownOpen = value;
                RaisePropertyChanged(() => ApplicationIsDropDownOpen);
                if (_applicationIsDropDownOpen)
                    OnApplicationsDropDownOpenedCommandExecute();
            }
        }

        private bool _tagIsDropDownOpen;
        public bool TagIsDropDownOpen
        {
            get { return _tagIsDropDownOpen; }
            set
            {
                if (value == _tagIsDropDownOpen)
                {
                    return;
                }
                _tagIsDropDownOpen = value;
                RaisePropertyChanged(() => TagIsDropDownOpen);
                if (_tagIsDropDownOpen)
                    OnTagIsDropDownOpenCommandExecute();
            }
        }
        #endregion
    }
}