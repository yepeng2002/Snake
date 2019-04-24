using DevExpress.Xpf.Mvvm;
using Snake.App.Controls.Mvvm;
using Snake.Client.WebApi;
using Snake.Core.Models;
using System;
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

            this.PageAppLog = new PageAppLog()
            {
                PageIndex = 1,
                PageSize = 40,
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
                    foreach (var item in result.Item2)
                        AppLogs.Add(item);
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
        #endregion
    }
}