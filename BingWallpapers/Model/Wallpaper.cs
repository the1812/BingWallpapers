using Ace;
using Ace.Files.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ace.Web;

namespace BingWallpapers.Model
{
    sealed class Wallpaper
    {
        private static List<string> downloadedHash = new List<string>();
        public static void ResetDownloadedHash() => downloadedHash.Clear();
        public string FriendlyLocaleName { get; private set; }
        public string LocaleName { get; private set; }
        public string FileName => $"{Date.ToShortDateString()}-{LocaleName}.jpg";
        public string FullFileName => $"{Settings.DownloadPath.Backslash()}{FileName}";
        private string InfoUrl => $"https://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt={LocaleName}";
        public string DownloadUrl { get; private set; }
        public bool IsDownloaded
        {
            get
            {
                var downloadFolder = new DirectoryInfo(Settings.DownloadPath);
                if (downloadFolder.Exists)
                {
                    if (downloadFolder.HasFile(FileName))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        public bool IsDownloading { get; private set; }
        public DateTime Date { get; private set; }
        public string Copyright { get; private set; }
        public string Hash { get; private set; }

        public Wallpaper(string locale, string friendlyName)
        {
            LocaleName = locale;
            FriendlyLocaleName = friendlyName;
        }
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        public Task Download()
        {
            if (IsDownloaded || IsDownloading)
            {
                return null;
            }
            return Task.Run(() =>
            {
                try
                {
                    using (var client = new WebClient())
                    {
                        IsDownloading = true;
                        //tokenSource = new CancellationTokenSource();
                        client.Encoding = Encoding.UTF8;

                        var info = client.DownloadStringAsTask(InfoUrl, tokenSource.Token).Result;
                        Debug.Assert(JsonObject.TryParse(info, out var json));
                        json = json["images"].ArrayValue[0].ObjectValue;
                        var timeString = json["fullstartdate"].StringValue;
                        Date = new DateTime(
                            timeString.Substring(0, 4).ToInt32(),
                            timeString.Substring(4, 2).ToInt32(),
                            timeString.Substring(6, 2).ToInt32(),
                            timeString.Substring(8, 2).ToInt32(),
                            timeString.Substring(10, 2).ToInt32(),
                            0);
                        DownloadUrl = $"https://www.bing.com{json["url"].StringValue}";
                        Copyright = json["copyright"].StringValue;
                        Hash = json["hsh"].StringValue;
                        if (!downloadedHash.Contains(Hash))
                        {
                            downloadedHash.Add(Hash);
                            Debug.WriteLine($"Hash added: {Hash}");
                            client.DownloadFileAsTask(DownloadUrl, FullFileName, tokenSource.Token).Wait();
                            Debug.WriteLine($"Downloaded: {LocaleName}");
                        }
                        else
                        {
                            Debug.WriteLine($"Hash conflict: {Hash}");
                        }
                    }
                }
                catch (Exception ex)
                when (ex is OperationCanceledException || ex is TaskCanceledException)
                {

                }
                finally
                {
                    IsDownloading = false;
                }
            });
        }
        public void CancelDownload() => tokenSource?.Cancel();
    }
}
