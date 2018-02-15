using Ace;
using Ace.Wpf.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpapers.Languages
{
    sealed class MainLanguage : Language<MainLanguage.Keys>
    {
        public enum Keys
        {
            WindowTitle,
            SettingsSaveFailedTitle,
            SettingsSaveFailedMessage,
        }
        public MainLanguage()
        {
            translations.AddRange(Translation.FromJson(
                new Uri($"../Languages/{nameof(MainLanguage)}.json", UriKind.Relative)));
        }
    }
}
