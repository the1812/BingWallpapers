using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BingWallpapers.Languages;
using BingWallpapers.View;

namespace BingWallpapers.ViewModel
{
    sealed partial class MainViewModel : ViewModel<MainView, MainLanguage>
    {
        public MainViewModel(MainView view) : base(view, new MainLanguage()) { }

    }
}
