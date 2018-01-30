using BingWallpapers.Languages;
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
        public CheckViewModel(CheckView view) : base(view, new CheckLanguage()) { }

    }
}
