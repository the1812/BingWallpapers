using Ace;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpapers.Model
{
    sealed class WallpaperInfo
    {
        public string FriendlyLocaleName { get; set; } = "Unknown";
        public string LocaleName { get; set; } = "Unknown";
        public string FileName
        {
            get
            {
                string fileName = null;
                if (FileNameChecker.IsValid(Settings.FileNameFormat))
                {
                    fileName = Settings.FileNameFormat;
                }
                else
                {
                    fileName = Settings.DefaultFileNameFormat;
                }
                fileName = fileName.Replace(Settings.FormatYear, Date.Year.ToString());
                fileName = fileName.Replace(Settings.FormatMonth, Date.Month.ToString("00"));
                fileName = fileName.Replace(Settings.FormatDay, Date.Day.ToString("00"));
                fileName = fileName.Replace(Settings.FormatLocale, LocaleName);

                return fileName + ".jpg";
            }
        }
        public string FullFileName => $"{Settings.DownloadPath.Backslash()}{FileName}";
        public string InfoUrl => $"https://global.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt={LocaleName}";
        public string DownloadUrl { get; set; }
        public DateTime Date { get; set; }
        public bool IsNewToday
        {
            get
            {
                var now = DateTime.Now;
                var result =
                    now.Year == Date.Year &&
                    now.Month == Date.Month &&
                    now.Day == Date.Day;
                Debug.WriteLineIf(!result, $"Old date: {Date.ToShortDateString()}");
                return result;
            }
        }
        public string Copyright { get; set; }
        public string Title { get; set; }
        public string Hash { get; set; }
    }
}
