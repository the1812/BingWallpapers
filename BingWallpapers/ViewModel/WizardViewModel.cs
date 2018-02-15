using Ace.Wpf.Mvvm;
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
    sealed partial class WizardViewModel : ViewModel<WizardView, WizardLanguage>
    {
        public WizardViewModel() => Language = new WizardLanguage();

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

        private string path = Settings.DownloadPath;
        public string Path
        {
            get => path;
            set
            {
                path = value;
                OnPropertyChanged(nameof(Path));
            }
        }

    }
}
