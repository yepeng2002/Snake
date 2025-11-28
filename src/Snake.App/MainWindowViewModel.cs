using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using MahApps.Metro;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Media;
using System.ComponentModel;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using Snake.App.Module.Models;
using Snake.App.Controls.Mvvm;
using Messenger = Snake.App.Controls.Mvvm.Messenger;

namespace Snake.App
{
    public class AccentColorMenuData
    {
        public string Name { get; set; }
        public Brush BorderColorBrush { get; set; }
        public Brush ColorBrush { get; set; }

        private ICommand changeAccentCommand;

        public ICommand ChangeAccentCommand
        {
            get { return this.changeAccentCommand ?? (changeAccentCommand = new SimpleCommand { CanExecuteDelegate = x => true, ExecuteDelegate = x => this.DoChangeTheme(x) }); }
        }

        protected virtual void DoChangeTheme(object sender)
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            var accent = ThemeManager.GetAccent(this.Name);
            ThemeManager.ChangeAppStyle(Application.Current, accent, theme.Item1);
        }
    }

    public class AppThemeMenuData : AccentColorMenuData
    {
        protected override void DoChangeTheme(object sender)
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            var appTheme = ThemeManager.GetAppTheme(this.Name);
            ThemeManager.ChangeAppStyle(Application.Current, theme.Item2, appTheme);
        }
    }

    public class MainWindowViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly IDialogCoordinator _dialogCoordinator;
        int? _integerGreater10Property;

        public MainWindowViewModel(IDialogCoordinator dialogCoordinator)
        {
            this.Title = "Snake Log";
            _dialogCoordinator = dialogCoordinator;
            
            // create metro theme color menu items for the demo
            this.AppThemes = ThemeManager.AppThemes
                                           .Select(a => new AppThemeMenuData() { Name = a.Name, BorderColorBrush = a.Resources["BlackColorBrush"] as Brush, ColorBrush = a.Resources["WhiteColorBrush"] as Brush })
                                           .ToList();
            
            BrushResources = FindBrushResources();

            CultureInfos = CultureInfo.GetCultures(CultureTypes.InstalledWin32Cultures).OrderBy(c => c.DisplayName).ToList();

            //注册主界面事件
            Messenger.Default.Register<StatusUpdateMessage>(this, OnStatusUpdateMessage);
        }

        public void Dispose()
        {
            Messenger.Default.Unregister<StatusUpdateMessage>(this, OnStatusUpdateMessage);
        }

        private void OnStatusUpdateMessage(StatusUpdateMessage statusUpdateMessage)
        {
            StatusContent = statusUpdateMessage.Message;
        }

        public string Title { get; set; }
        public int SelectedIndex { get; set; }
        public List<AppThemeMenuData> AppThemes { get; set; }
        public List<CultureInfo> CultureInfos { get; set; }

        private string _statusContent = "Ready";

        public string StatusContent
        {
            get { return this._statusContent; }
            set
            {
                if (Equals(value, this._statusContent))
                {
                    return;
                }
                this._statusContent = value;
                this.RaisePropertyChanged("StatusContent");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event if needed.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        
        public IEnumerable<string> BrushResources { get; private set; }
        
        private IEnumerable<string> FindBrushResources()
        {
            var rd = new ResourceDictionary
            {
                Source = new Uri(@"/MahApps.Metro;component/Styles/Colors.xaml", UriKind.RelativeOrAbsolute)
            };

            var resources = rd.Keys.Cast<object>()
                    .Where(key => rd[key] is SolidColorBrush)
                    .Select(key => key.ToString())
                    .OrderBy(s => s)
                    .ToList();

            return resources;
        }
    }
}
