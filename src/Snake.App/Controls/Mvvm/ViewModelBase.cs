namespace Snake.App.Controls.Mvvm
{
    /// <summary>
    /// ViewModel基类
    /// </summary>
    public class ViewModelBase : BaseNotifyPropertyChanged
    {
        #region 属性

        private ViewModelStatus _status = ViewModelStatus.None;
        /// <summary>
        /// ViewModel状态
        /// </summary>
        public ViewModelStatus Status
        {
            get { return _status; }
            set { this._status = value; base.OnPropertyChanged(() => this.Status); }
        }

        private bool _isEnable = true;
        public bool IsEnable
        {
            get { return _isEnable; }
            set { this._isEnable = value; base.OnPropertyChanged(() => this.IsEnable); }
        }

        private bool _isFirstLoad = true;
        /// <summary>
        /// 是否第一次加载
        /// </summary>
        public bool IsFirstLoad
        {
            get { return _isFirstLoad; }
            set { this._isFirstLoad = value; base.OnPropertyChanged(() => this.IsFirstLoad); }
        }

        #endregion


        #region  ProgressBar

        private int _progressVaule = 10;
        public int ProgressValue
        {
            get { return _progressVaule; }
            set { this._progressVaule = value; base.OnPropertyChanged(() => this.ProgressValue); }
        }

        private int _progressMin = 0;
        public int ProgressMin
        {
            get { return _progressMin; }
            set { this._progressMin = value; base.OnPropertyChanged(() => this.ProgressMin); }
        }

        private int _progressMax = 20;
        public int ProgressMax
        {
            get { return _progressMax; }
            set { this._progressMax = value; base.OnPropertyChanged(() => this.ProgressMax); }
        }

        protected void ProgressStep(int step)
        {
            if (step == 0)
            {
                ProgressValue = 0;
                return;
            }

            if (step + _progressVaule < _progressMax)
                ProgressValue += step;
            else
                ProgressValue = _progressMax;
        }
        #endregion

    }
}
