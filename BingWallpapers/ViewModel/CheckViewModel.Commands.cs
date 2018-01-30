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
            ExecuteAction = o => cancelCheck(),
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
