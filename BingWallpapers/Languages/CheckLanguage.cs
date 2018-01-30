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
                "CheckingLocale",
                "Complete",
                "CompleteMessage",
                "CompleteTitle",
                "Canceling",
                "CanceledTitle",
                "DownloadingLocale",
            };
            simplifiedChinese = new List<string>
            {
                "正在检查新壁纸",
                "取消",
                "正在检查 {0}",
                "完成",
                "检查了{0}个壁纸。",
                "检查完成",
                "取消中",
                "已取消",
                "正在下载 {0}",
            };
            americanEnglish = new List<string>
            {
                "Checking for new wallpapers",
                "Cancel",
                "Checking {0}",
                "Complete",
                "Checked {0} wallpaper(s).",
                "Check complete",
                "Canceling",
                "Cenceled",
                "Downloading from {0}",
            };
        }
    }
}
