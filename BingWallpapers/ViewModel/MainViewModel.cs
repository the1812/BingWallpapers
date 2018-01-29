using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BingWallpapers.Languages;
using BingWallpapers.View;
using Ace.Wpf;
using System.Windows.Media;
using System.Windows;

namespace BingWallpapers.ViewModel
{
    sealed partial class MainViewModel : ViewModel<MainView, MainLanguage>
    {
        public MainViewModel(MainView view) : base(view, new MainLanguage())
        {
            view.OnColorizationChanged(color =>
            {
                Application.Current.Resources["DwmColor"] = new SolidColorBrush(color);
                Application.Current.Resources["TitleBarColor"] = new SolidColorBrush(DwmEffect.WindowTitleColor);
            });
        }

    }
}
