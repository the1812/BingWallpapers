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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using BingWallpapers.Model;
using System.IO;

namespace BingWallpapers.ViewModel
{
    sealed partial class MainViewModel : ViewModel<MainView, MainLanguage>
    {
        public MainViewModel(MainView view) : base(view, new MainLanguage())
        {
            if (DwmEffect.WindowTitleColor == Colors.White)
            {
                view.Icon = new BitmapImage(new Uri("../Bing.Logo.ico", UriKind.Relative));
            }
            view.Loaded += (s, e) =>
            {
                view.OnColorizationChanged(color =>
                {
                    Application.Current.Resources["DwmSelectionBrush"] = new SolidColorBrush(color) { Opacity = 0.5 };
                    Application.Current.Resources["DwmBrush"] = new SolidColorBrush(color);
                    Application.Current.Resources["TitleBarColor"] = new SolidColorBrush(DwmEffect.WindowTitleColor);
                });
                if (Directory.Exists(Settings.DownloadPath))
                {
                    Navigate(new Uri("../View/CheckView.xaml", UriKind.Relative));
                }
                else
                {
                    Navigate(new Uri("../View/WizardView.xaml", UriKind.Relative));
                }
            };
            Current = this;
        }
        public static MainViewModel Current { get; private set; }
        private Storyboard FadeIn => View.FindResource("FadeIn") as Storyboard;
        private Storyboard FadeOut => View.FindResource("FadeOut") as Storyboard;

        //private double frameOpacity = 0;
        //public double FrameOpacity
        //{
        //    get => frameOpacity;
        //    set
        //    {
        //        frameOpacity = value;
        //        OnPropertyChanged(nameof(FrameOpacity));
        //    }
        //}

        private Uri frameSource = null;
        public Uri FrameSource
        {
            get => frameSource;
            set
            {
                frameSource = value;
                OnPropertyChanged(nameof(FrameSource));
            }
        }

        public void Navigate(Uri page)
        {
            var fadeOut = FadeOut;
            fadeOut.Completed += (s, e) =>
            {
                FrameSource = page;
                FadeIn.Begin();
            };
            fadeOut.Begin();
        }
    }
}
