using Ace.Files.Json;
using BingWallpapers.Languages;
using BingWallpapers.Model;
using BingWallpapers.View;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BingWallpapers.ViewModel
{
    sealed partial class CheckViewModel : ViewModel<CheckView, CheckLanguage>
    {
        public CheckViewModel(CheckView view) : base(view, new CheckLanguage())
        {
            LocaleCount = Locales.Dictionary.Count * 3;
            Title = this["Checking"];
            Message = "";
            CancelButtonText = this["Cancel"];
            startCheck();
        }

        private string title;
        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private string message;
        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged(nameof(Message));
            }
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
                View.SetProgress(value);
                OnPropertyChanged(nameof(CheckedLocale));
            }
        }

        private Visibility cancelButtonVisibility = Visibility.Visible;
        public Visibility CancelButtonVisibility
        {
            get => cancelButtonVisibility;
            set
            {
                cancelButtonVisibility = value;
                OnPropertyChanged(nameof(CancelButtonVisibility));
            }
        }
        private Visibility completeButtonVisibility = Visibility.Collapsed;
        public Visibility CompleteButtonVisibility
        {
            get => completeButtonVisibility;
            set
            {
                completeButtonVisibility = value;
                OnPropertyChanged(nameof(CompleteButtonVisibility));
            }
        }
        private bool isButtonEnabled = true;
        public bool IsButtonEnabled
        {
            get => isButtonEnabled;
            set
            {
                isButtonEnabled = value;
                OnPropertyChanged(nameof(IsButtonEnabled));
            }
        }

        private string cancelButtonText;
        public string CancelButtonText
        {
            get => cancelButtonText;
            set
            {
                cancelButtonText = value;
                OnPropertyChanged(nameof(CancelButtonText));
            }
        }


        private bool canceled = false;
        private List<Wallpaper> wallpapers = null;
        private async void startCheck()
        {
            canceled = false;
            IsButtonEnabled = false;
            wallpapers = Locales.Wallpapers;
            Wallpaper.ResetHash();
            CheckedLocale = 0;
            try
            {
                foreach (var wallpaper in wallpapers)
                {
                    if (canceled)
                    {
                        break;
                    }
                    Title = String.Format(this["CheckingLocale"], wallpaper.FriendlyLocaleName);
                    await wallpaper.DownloadInfo();
                    CheckedLocale += 2;
                }
                foreach (var wallpaper in wallpapers)
                {
                    if (canceled)
                    {
                        break;
                    }
                    Title = String.Format(this["DownloadingLocale"], wallpaper.FriendlyLocaleName);
                    await wallpaper.Download();
                    CheckedLocale++;
                }
                if (!canceled)
                {
                    Title = this["CompleteTitle"];
                }
                else
                {
                    await Task.Delay(500);
                    Title = this["CanceledTitle"];
                    CheckedLocale = LocaleCount * 3;
                }
                Message = String.Format(this["CompleteMessage"], Wallpaper.DownloadedCount);
            }
            catch (Exception ex)
            when (ex is WebException || (ex is AggregateException && ex.InnerException is WebException))
            {
                Title = this["FailedTitle"];
                Message = ex.Message;
            }
            finally
            {
                CancelButtonVisibility = Visibility.Collapsed;
                CompleteButtonVisibility = Visibility.Visible;
                IsButtonEnabled = true;
            }
        }
        private void cancelCheck()
        {
            canceled = true;
            IsButtonEnabled = false;
            wallpapers?.ForEach(w => w.CancelDownload());
            CancelButtonText = this["Canceling"];
        }
    }
}
