using Ace.Wpf.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BingWallpapers.ViewModel
{
    partial class CheckViewModel
    {
        public BindingCommand CancelCommand => new BindingCommand
        {
            ExecuteAction = o =>
            {
                canceled = true;
                IsButtonEnabled = false;
                wallpapers?.ForEach(w => w.CancelDownload());
                CancelButtonText = this["Canceling"];
            },
        };
        public BindingCommand CompleteCommand => new BindingCommand
        {
            ExecuteAction = async o =>
            {
                IsButtonEnabled = false;
                await Task.Delay(500);
                Application.Current.Shutdown();
            },
        };
    }
}
