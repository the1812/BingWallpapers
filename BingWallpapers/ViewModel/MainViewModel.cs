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
using System.Windows.Controls;

namespace BingWallpapers.ViewModel
{
    sealed partial class MainViewModel : ViewModel<MainView, MainLanguage>
    {
        public MainViewModel(MainView view) : base(view, new MainLanguage())
        {
            view.Loaded += (s, e) =>
            {
                view.OnColorizationChanged(color =>
                {
                    Application.Current.Resources["DwmSelectionBrush"] = new SolidColorBrush(color) { Opacity = 0.5 };
                    Application.Current.Resources["DwmBrush"] = new SolidColorBrush(color);
                    Application.Current.Resources["TitleBarColor"] = new SolidColorBrush(DwmEffect.WindowTitleColor);
                });
            };
        }

        //private Page source = new WizardView();
        //public Page Source
        //{
        //    get => source;
        //    set
        //    {
        //        source = value;
        //        OnPropertyChanged(nameof(Source));
        //    }
        //}

    }
}
