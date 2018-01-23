using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpapers.Model.Languages
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
                "窗口标题",
            };
            americanEnglish = new List<string>
            {
                "Window Title",
            };
        }
    }
}
