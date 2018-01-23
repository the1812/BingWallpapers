using BingWallpapers.Languages;
using BingWallpapers.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpapers.ViewModel
{
    sealed class WizardViewModel : ViewModel<WizardView, WizardLanguage>
    {
        public WizardViewModel(WizardView view) : base(view, new WizardLanguage()) { }
    }
}
