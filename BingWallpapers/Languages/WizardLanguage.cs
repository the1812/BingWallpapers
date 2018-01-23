using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpapers.Languages
{
    sealed class WizardLanguage : Language
    {
        protected override void LoadTranslationList()
        {
            keys = new List<string>
            {
                "WelcomeText",
            };
            simplifiedChinese = new List<string>
            {
                "欢迎使用",
            };
            americanEnglish = new List<string>
            {
                "Welcome",
            };
        }
    }
}
