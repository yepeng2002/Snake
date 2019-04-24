using DevExpress.Xpf.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.App.Controls.Mvvm
{
    /// <summary>
    /// ViewModel基类
    /// </summary>
    public class ViewModelBase : BindableBase
    {
        #region 属性

        private ViewModelStatus _status = ViewModelStatus.None;
        /// <summary>
        /// ViewModel状态
        /// </summary>
        public ViewModelStatus Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value, () => Status); }
        }

        private bool _isEnable = true;
        public bool IsEnable
        {
            get { return _isEnable; }
            set { SetProperty(ref _isEnable, value, () => IsEnable); }
        }

        private bool _isFirstLoad = true;
        /// <summary>
        /// 是否第一次加载
        /// </summary>
        public bool IsFirstLoad
        {
            get { return _isFirstLoad; }
            set { SetProperty(ref _isFirstLoad, value, () => IsFirstLoad); }
        }

        #endregion


        #region  ProgressBar

        private int _progressVaule = 10;
        public int ProgressValue
        {
            get { return _progressVaule; }
            set { SetProperty(ref _progressVaule, value, () => ProgressValue); }
        }

        private int _progressMin = 0;
        public int ProgressMin
        {
            get { return _progressMin; }
            set { SetProperty(ref _progressMin, value, () => ProgressMin); }
        }

        private int _progressMax = 20;
        public int ProgressMax
        {
            get { return _progressMax; }
            set { SetProperty(ref _progressMax, value, () => ProgressMax); }
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
