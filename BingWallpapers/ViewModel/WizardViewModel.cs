using BingWallpapers.Languages;
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
        public WizardViewModel(WizardView view) : base(view, new WizardLanguage()) { }

        private string path;
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
