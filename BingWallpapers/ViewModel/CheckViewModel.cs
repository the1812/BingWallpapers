using Ace.Files.Json;
using BingWallpapers.Languages;
using BingWallpapers.Model;
using BingWallpapers.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpapers.ViewModel
{
    sealed partial class CheckViewModel : ViewModel<CheckView, CheckLanguage>
    {
        public CheckViewModel(CheckView view) : base(view, new CheckLanguage())
        {
            LocaleCount = Locales.Dictionary.Count;
            startCheck();
        }

        private double localeCount = 0.0;
        public double LocaleCount
        {
            get => localeCount;
            set
            {
                localeCount = value;
                OnPropertyChanged(nameof(LocaleCount));
            }
        }

        private double checkedLocale = 0.0;
        public double CheckedLocale
        {
            get => checkedLocale;
            set
            {
                checkedLocale = value;
                OnPropertyChanged(nameof(CheckedLocale));
            }
        }

        private List<Wallpaper> wallpapers = null;
        private async void startCheck()
        {
            wallpapers = Locales.Wallpapers;
            CheckedLocale = 0;
            await Task.Run(() =>
            {
                foreach (var wallpaper in wallpapers)
                {
                    wallpaper.Download();
                    CheckedLocale++;
                }
            });
        }
        private void cancelCheck() => wallpapers?.ForEach(w => w.CancelDownload());
    }
}
