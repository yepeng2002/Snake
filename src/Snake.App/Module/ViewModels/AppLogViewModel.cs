using Snake.App.Controls.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.App.Module.ViewModels
{
    public class AppLogViewModel : ViewModelBase
    {
        public AppLogViewModel()
        {

        }

        #region  属性
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
        #endregion
    }
}