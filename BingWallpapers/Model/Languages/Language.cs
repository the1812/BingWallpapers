using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ace;
using System.Globalization;

namespace BingWallpapers.Model.Languages
{
    abstract class Language : NotificationObject
    {
        protected List<string> DefaultLanguage => americanEnglish;
        protected List<string> keys;
        protected List<string> simplifiedChinese;
        protected List<string> americanEnglish;
        private Dictionary<string, string> cachedDictionary = null;

        public Dictionary<string, string> Dictionary
        {
            get
            {
                switch (Culture?.Name)
                {
                    case "zh-CN":
                        return keys.MakeDictionary(simplifiedChinese);
                    case "en-US":
                        return keys.MakeDictionary(americanEnglish);
                    default:
                        return keys.MakeDictionary(DefaultLanguage);
                }
            }
        }
        public string this[string key]
        {
            get
            {
                if (cachedDictionary != null)
                {
                    return cachedDictionary[key];
                }
                else
                {
                    reloadCulture();
                    return cachedDictionary[key];
                }
            }
        }
        private CultureInfo culture;
        public CultureInfo Culture
        {
            get => culture;
            set
            {
                culture = value;
                reloadCulture();
                OnPropertyChanged(nameof(Culture));
            }
        }

        public Language(CultureInfo cultureInfo)
        {
            LoadTranslationList();
            Culture = cultureInfo;
        }
        public Language()
        {
            LoadTranslationList();
            //Culture = new CultureInfo("en-US");
            Culture = CultureInfo.CurrentUICulture;
        }
        protected abstract void LoadTranslationList();
        private void reloadCulture()
        {
            cachedDictionary = Dictionary;
        }
    }
}
