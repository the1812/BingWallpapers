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
                "Title",
                "OK",
                "Browse",
                "Error",
                "PathNotExist",
            };
            simplifiedChinese = new List<string>
            {
                "下载位置设定",
                "确定",
                "浏览",
                "错误",
                "下载位置不存在。",
            };
            americanEnglish = new List<string>
            {
                "Select Download Folder",
                "OK",
                "Browse",
                "Error",
                "Download folder not exist.",
            };
        }
    }
}
