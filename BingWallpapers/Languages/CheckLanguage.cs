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
                "Cancel",
            };
            simplifiedChinese = new List<string>
            {
                "正在检查新壁纸",
                "取消",
            };
            americanEnglish = new List<string>
            {
                "Checking for new wallpapers",
                "Cancel",
            };
        }
    }
}
