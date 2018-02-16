using BingWallpapers.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpapers
{
    static class SilentChecker
    {
        public static void Start()
        {
            if (!Directory.Exists(Settings.DownloadPath))
            {
                return;
            }
            var wallpapers = Locales.Wallpapers;
            Wallpaper.ResetDownloadedInfo().Wait();
            try
            {
                foreach (var wallpaper in wallpapers)
                {
                    wallpaper.DownloadInfo().Wait();
                }
                foreach (var wallpaper in wallpapers)
                {
                    wallpaper.Download().Wait();
                }
            }
            catch (Exception ex)
            when (ex is WebException || (ex is AggregateException && ex.InnerException is WebException))
            {
                return;
            }
        }
    }
}
