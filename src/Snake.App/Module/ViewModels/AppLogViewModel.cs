﻿using DevExpress.Xpf.Mvvm;
using Snake.App.Controls.Mvvm;
using Snake.Client.WebApi;
using Snake.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Snake.App.Module.ViewModels
{
    public class AppLogViewModel : Controls.Mvvm.ViewModelBase
    {
        public AppLogViewModel()
        {
            //Set Command
            OnViewLoadedCommand = new DelegateCommand(OnViewLoadedCommandExecute);
            OnQueryCommand = new DelegateCommand(OnQueryCommandExecute);
            OnQueryNextPageCommand = new DelegateCommand(OnQueryNextPageCommandExecute);

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
            SnakeWebApiHttpProxy proxy = new SnakeWebApiHttpProxy();
            var result = await proxy.GetAppLogsPageAsync<IList<AppLog>>(this.PageAppLog);
            if (result.Item1 && result.Item2 != null)
                AppLogs = new ObservableCollection<AppLog>(result.Item2);
            else
                AppLogs = null;
        }

        #endregion


        #region command
        /// <summary>
        /// 视图首次加载
        /// </summary>
        public ICommand OnViewLoadedCommand { get; private set; }
        public ICommand OnQueryCommand { get; private set; }
        public ICommand OnQueryNextPageCommand { get; private set; }


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
                SnakeWebApiHttpProxy proxy = new SnakeWebApiHttpProxy();
                var result = await proxy.GetAppLogsPageAsync<IList<AppLog>>(this.PageAppLog);
                if (result.Item1 && result.Item2 != null)
                {
                    var list = AppLogs.Union<AppLog>(result.Item2).ToList<AppLog>();
                    AppLogs = new ObservableCollection<AppLog>(list);
                }
            }
            finally
            {
                Status = ViewModelStatus.Loaded;
            }
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

        private ObservableCollection<string> _appications;
        public ObservableCollection<string> Appications
        {
            get { return _appications; }
            set
            {
                if (value == _appications)
                {
                    return;
                }
                _appications = value;
                RaisePropertyChanged(() => Appications);
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
        #endregion
    }
}