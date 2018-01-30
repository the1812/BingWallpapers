using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpapers.Languages
{
    sealed class CheckLanguage : Language
    {
        protected override void LoadTranslationList()
        {
            keys = new List<string>
            {
                "Checking",
            };
            simplifiedChinese = new List<string>
            {
                "正在检查{0}",
            };
            americanEnglish = new List<string>
            {
                "Checking {0}",
            };
        }
    }
}
