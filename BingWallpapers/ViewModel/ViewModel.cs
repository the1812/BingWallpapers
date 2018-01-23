using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BingWallpapers.Model.Languages;

namespace BingWallpapers.ViewModel
{
    abstract class ViewModel<TView, TLanguage> : NotificationObject
        where TView : FrameworkElement
        where TLanguage : Language
    {
        public ViewModel(TView view, TLanguage language)
        {
            View = view;
            Language = language;
        }
        public TView View { get; private set; }
        protected TLanguage Language { get; private set; }
    }
}
