using Ace.Files.Json;
using Ace.Wpf.Mvvm;
using BingWallpapers.Languages;
using BingWallpapers.Model;
using BingWallpapers.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace BingWallpapers.ViewModel
{
    sealed partial class CheckViewModel : ViewModel<CheckView, CheckLanguage>
    {
        public CheckViewModel(CheckView view)
        {
            View = view;
            Language = new CheckLanguage();
            LocaleCount = Locales.Dictionary.Count * 3;
            Title = Language[CheckLanguage.Keys.Checking];
            Message = "";
            CancelButtonText = Language[CheckLanguage.Keys.Cancel];
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
            wallpapers = Locales.Wallpapers;
            Title = Language[CheckLanguage.Keys.Preparing];
            await Wallpaper.ResetDownloadedInfo();
            CheckedLocale = 0;
            try
            {
                if (!Directory.Exists(Settings.DownloadPath))
                {
                    throw new InvalidOperationException(new WizardLanguage()[WizardLanguage.Keys.PathNotExist]);
                }
                foreach (var wallpaper in wallpapers)
                {
                    if (canceled)
                    {
                        //currentWallpaper = null;
                        break;
                    }
                    //currentWallpaper = wallpaper;
                    Title = String.Format(
                        Language[CheckLanguage.Keys.CheckingLocale],
                        wallpaper.Info.FriendlyLocaleName);
                    await wallpaper.DownloadInfo();
                    CheckedLocale++;
                }
                foreach (var wallpaper in wallpapers)
                {
                    if (canceled)
                    {
                        //currentWallpaper = null;
                        break;
                    }
                    //currentWallpaper = wallpaper;
                    Title = String.Format(
                        Language[CheckLanguage.Keys.DownloadingLocale],
                        wallpaper.Info.FriendlyLocaleName);
                    await wallpaper.Download();
                    CheckedLocale += 2;
                }
                if (!canceled)
                {
                    Title = Language[CheckLanguage.Keys.CompleteTitle];
                }
                else
                {
                    await Task.Delay(500);
                    Title = Language[CheckLanguage.Keys.CanceledTitle];
                    CheckedLocale = LocaleCount * 2;
                }
                Message = String.Format(
                    Language[CheckLanguage.Keys.CompleteMessage], 
                    Wallpaper.DownloadedCount);
            }
            catch (Exception ex)
            when (ex is WebException ||
                ex is InvalidOperationException ||
                (ex is AggregateException && ex.InnerException is WebException))
            {
                Title = Language[CheckLanguage.Keys.FailedTitle];
                Message = ex.Message;
            }
#if !DEBUG
            catch (Exception ex)
            {
                Title = Language[CheckLanguage.Keys.FailedTitle];
                Message = ex.Message;
            }
#endif
            finally
            {
                    CancelButtonVisibility = Visibility.Collapsed;
                    CompleteButtonVisibility = Visibility.Visible;
                    IsButtonEnabled = true;
            }
        }
    }
}
