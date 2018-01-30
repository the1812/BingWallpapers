using BingWallpapers.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BingWallpapers
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Settings.Load();
            base.OnStartup(e);
        }
        protected override void OnExit(ExitEventArgs e)
        {
            Settings.Save();
            base.OnExit(e);
        }
    }
}
