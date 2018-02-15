using Ace.Wpf.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpapers.Languages
{
    sealed class CheckLanguage : Language<CheckLanguage.Keys>
    {
        public enum Keys
        {
            Checking,
            Cancel,
            CheckingLocale,
            Complete,
            CompleteMessage,
            CompleteTitle,
            Canceling,
            CanceledTitle,
            DownloadingLocale,
            FailedTitle
        }
        public CheckLanguage()
        {
            translations.AddRange(Translation.FromJson(
                new Uri($"../Languages/{nameof(CheckLanguage)}.json", UriKind.Relative)));
        }
    }
}
