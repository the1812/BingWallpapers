using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpapers.Languages
{
    sealed class MainLanguage : Language
    {
        protected override void LoadTranslationList()
        {
            keys = new List<string>
            {
                "WindowTitle",
            };
            simplifiedChinese = new List<string>
            {
                "Bing 壁纸",
            };
            americanEnglish = new List<string>
            {
                "Bing Wallpapers",
            };
        }
    }
}
