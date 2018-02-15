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
                "SettingsSaveFailedTitle",
                "SettingsSaveFailedMessage",
            };
            simplifiedChinese = new List<string>
            {
                "Bing 壁纸",
                "错误",
                "无法保存设置。",
            };
            americanEnglish = new List<string>
            {
                "Bing Wallpapers",
                "Error",
                "Cannot save settings.",
            };
        }
    }
}
