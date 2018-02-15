using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ace;
using Ace.Wpf.Mvvm;

namespace BingWallpapers.Languages
{
    sealed class WizardLanguage : Language<WizardLanguage.Keys>
    {
        public enum Keys
        {
            Title,
            OK,
            Browse,
            Error,
            PathNotExist,
            RestrictedNetwork,
            FileNameFormat,
        }
        public WizardLanguage()
        {
            translations.AddRange(Translation.FromJson(
                new Uri($"../Languages/{nameof(WizardLanguage)}.json", UriKind.Relative)));
        }
    }
}
