using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MahApps.Metro;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using NHotkey;
using NHotkey.Wpf;
using System.Windows.Media;
using System.ComponentModel;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using Snake.App.Module.Models;
using System.Windows.Controls;
using Snake.App.Utilities;
using System.Threading;
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

    public class MainWindowViewModel : INotifyPropertyChanged, IDataErrorInfo, IDisposable
    {
        private readonly IDialogCoordinator _dialogCoordinator;
        int? _integerGreater10Property;
        private bool _animateOnPositionChange = true;

        public MainWindowViewModel(IDialogCoordinator dialogCoordinator)
        {
            this.Title = "Snake Log";
            _dialogCoordinator = dialogCoordinator;
            
            // create metro theme color menu items for the demo
            this.AppThemes = ThemeManager.AppThemes
                                           .Select(a => new AppThemeMenuData() { Name = a.Name, BorderColorBrush = a.Resources["BlackColorBrush"] as Brush, ColorBrush = a.Resources["WhiteColorBrush"] as Brush })
                                           .ToList();

            FlipViewImages = new Uri[]
                             {
                                 new Uri("http://www.public-domain-photos.com/free-stock-photos-4/landscapes/mountains/painted-desert.jpg", UriKind.Absolute),
                                 new Uri("http://www.public-domain-photos.com/free-stock-photos-3/landscapes/forest/breaking-the-clouds-on-winter-day.jpg", UriKind.Absolute),
                                 new Uri("http://www.public-domain-photos.com/free-stock-photos-4/travel/bodie/bodie-streets.jpg", UriKind.Absolute)
                             };

            BrushResources = FindBrushResources();

            CultureInfos = CultureInfo.GetCultures(CultureTypes.InstalledWin32Cultures).OrderBy(c => c.DisplayName).ToList();

            try
            {
                HotkeyManager.Current.AddOrReplace("demo", HotKey.Key, HotKey.ModifierKeys, (sender, e) => OnHotKey(sender, e));
            }
            catch (HotkeyAlreadyRegisteredException exception)
            {
                System.Diagnostics.Trace.TraceWarning("Uups, the hotkey {0} is already registered!", exception.Name);
            }

            //注册主界面事件
            //DevExpress.Xpf.Mvvm.Messenger.Default.Register<StatusUpdateMessage>(this, OnStatusUpdateMessage);
            Messenger.Default.Register<StatusUpdateMessage>(this, OnStatusUpdateMessage);
        }

        public void Dispose()
        {
            HotkeyManager.Current.Remove("demo");
            //DevExpress.Xpf.Mvvm.Messenger.Default.Unregister<StatusUpdateMessage>(this, OnStatusUpdateMessage);
            Messenger.Default.Unregister<StatusUpdateMessage>(this, OnStatusUpdateMessage);
        }

        private void OnStatusUpdateMessage(StatusUpdateMessage statusUpdateMessage)
        {
            StatusContent = statusUpdateMessage.Message;
        }

        #region IDataErrorInfo

        public string this[string columnName]
        {
            get
            {
                if (columnName == "IntegerGreater10Property" && this.IntegerGreater10Property < 10)
                {
                    return "Number is not greater than 10!";
                }

                if (columnName == "DatePickerDate" && this.DatePickerDate == null)
                {
                    return "No date given!";
                }

                if (columnName == "HotKey" && this.HotKey != null && this.HotKey.Key == Key.D && this.HotKey.ModifierKeys == ModifierKeys.Shift)
                {
                    return "SHIFT-D is not allowed";
                }

                return null;
            }
        }

        [Description("Test-Property")]
        public string Error { get { return string.Empty; } }

        #endregion

        public string Title { get; set; }
        public int SelectedIndex { get; set; }
        public List<AppThemeMenuData> AppThemes { get; set; }
        public List<CultureInfo> CultureInfos { get; set; }

        public int? IntegerGreater10Property
        {
            get { return this._integerGreater10Property; }
            set
            {
                if (Equals(value, _integerGreater10Property))
                {
                    return;
                }

                _integerGreater10Property = value;
                RaisePropertyChanged("IntegerGreater10Property");
            }
        }

        DateTime? _datePickerDate;

        [Display(Prompt = "Auto resolved Watermark")]
        public DateTime? DatePickerDate
        {
            get { return this._datePickerDate; }
            set
            {
                if (Equals(value, _datePickerDate))
                {
                    return;
                }
                _datePickerDate = value;
                RaisePropertyChanged("DatePickerDate");
            }
        }

        private bool _quitConfirmationEnabled;
        public bool QuitConfirmationEnabled
        {
            get { return _quitConfirmationEnabled; }
            set
            {
                if (value.Equals(_quitConfirmationEnabled)) return;
                _quitConfirmationEnabled = value;
                RaisePropertyChanged("QuitConfirmationEnabled");
            }
        }

        private bool showMyTitleBar = true;
        public bool ShowMyTitleBar
        {
            get { return showMyTitleBar; }
            set
            {
                if (value.Equals(showMyTitleBar)) return;
                showMyTitleBar = value;
                RaisePropertyChanged("ShowMyTitleBar");
            }
        }

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

        public Uri[] FlipViewImages
        {
            get;
            set;
        }


        public class RandomDataTemplateSelector : DataTemplateSelector
        {
            public DataTemplate TemplateOne { get; set; }

            public override DataTemplate SelectTemplate(object item, DependencyObject container)
            {
                return TemplateOne;
            }
        }

        private HotKey _hotKey = new HotKey(Key.Home, ModifierKeys.Control | ModifierKeys.Shift);

        public HotKey HotKey
        {
            get { return _hotKey; }
            set
            {
                if (_hotKey != value)
                {
                    _hotKey = value;
                    if (_hotKey != null && _hotKey.Key != Key.None)
                    {
                        HotkeyManager.Current.AddOrReplace("demo", HotKey.Key, HotKey.ModifierKeys, (sender, e) => OnHotKey(sender, e));
                    }
                    else
                    {
                        HotkeyManager.Current.Remove("demo");
                    }
                    RaisePropertyChanged("HotKey");
                }
            }
        }

        private async Task OnHotKey(object sender, HotkeyEventArgs e)
        {
            await ((MetroWindow)Application.Current.MainWindow).ShowMessageAsync(
                "Hotkey pressed",
                "You pressed the hotkey '" + HotKey + "' registered with the name '" + e.Name + "'");
        }

        private ICommand toggleIconScalingCommand;

        public ICommand ToggleIconScalingCommand
        {
            get
            {
                return toggleIconScalingCommand ?? (toggleIconScalingCommand = new SimpleCommand
                {
                    ExecuteDelegate = ToggleIconScaling
                });
            }
        }

        private void ToggleIconScaling(object obj)
        {
            var multiFrameImageMode = (MultiFrameImageMode)obj;
            ((MetroWindow)Application.Current.MainWindow).IconScalingMode = multiFrameImageMode;
            RaisePropertyChanged("IsScaleDownLargerFrame");
            RaisePropertyChanged("IsNoScaleSmallerFrame");
        }
    }
}
