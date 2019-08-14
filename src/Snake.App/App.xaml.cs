using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Snake.App
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //********************** Ioc & Servicebus ********************
            ApplicationBootstrapper.RegisterContainer();
            
            //启动主窗体
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
