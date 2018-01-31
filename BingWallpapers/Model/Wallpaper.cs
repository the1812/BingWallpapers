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
        private static HashSet<string> jsonHash = new HashSet<string>();
        private static HashSet<byte[]> imageHash = new HashSet<byte[]>();
        public static void ResetHash()
        {
            jsonHash.Clear();
            imageHash.Clear();
            DownloadedCount = 0;
            foreach (var file in new DirectoryInfo(Settings.DownloadPath).EnumerateFiles())
            {
                imageHash.Add(ImageProcesser.LoadFromFile(file.FullName));
            }
        }
        public static int DownloadedCount { get; private set; } = 0;
        private static bool containsData(byte[] data)
        {
            foreach (var image in imageHash)
            {
                if (Enumerable.SequenceEqual(data, image))
                {
                    Debug.WriteLine($"Data conflict: {data.GetHashCode()}");
                    return true;
                }
            }
            return false;
        }
        public string FriendlyLocaleName { get; private set; } = "Unknown";
        public string LocaleName { get; private set; } = "Unknown";
        public string FileName => $"{Date.ToShortDateString()}-{LocaleName}.jpg";
        public string FullFileName => $"{Settings.DownloadPath.Backslash()}{FileName}";
        private string InfoUrl => $"https://global.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt={LocaleName}";
        public string DownloadUrl { get; private set; }
        public bool IsInfoDownloaded { get; private set; }
        public bool IsInfoDownloading { get; private set; }
        public bool IsDownloaded => File.Exists(FullFileName);
        public bool IsDownloading { get; private set; }
        public DateTime Date { get; private set; }
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
        public string Copyright { get; private set; }
        public string Hash { get; private set; }
        private WebClient getWebClient()
        {
            var webClient = new WebClient
            {
                Encoding = Encoding.UTF8,
            };
            //var headers = webClient.Headers;
            //headers["Accept"] = "text/html, application/xhtml+xml, image/jxr, */*";
            //headers["Accept-Encoding"] = "gzip, deflate, br";
            //headers["Accept-Language"] = LocaleName;
            //headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299";
            return webClient;
        }

        public Wallpaper(string locale, string friendlyName)
        {
            LocaleName = locale;
            FriendlyLocaleName = friendlyName;
        }
        public Task DownloadInfo()
        {
            return Task.Run(() =>
            {
                if (IsInfoDownloaded || IsInfoDownloading)
                {
                    return;
                }
                try
                {
                    using (var client = getWebClient())
                    {
                        IsInfoDownloading = true;
                        var info = client.DownloadStringAsTask(InfoUrl, tokenSource.Token).Result;
                        var parseResult = JsonObject.TryParse(info, out var json);
                        Debug.Assert(parseResult);
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
                        Debug.WriteLineIf(IsDownloaded, $"Already downloaded: {LocaleName}-{Hash}");
                        IsInfoDownloaded = true;
                    }
                }
                catch (AggregateException ex)
                when (ex.InnerException is WebException)
                {
                    return;
                }
                catch (Exception ex)
                when (ex is OperationCanceledException || ex is TaskCanceledException)
                {
                    return;
                }
#if !DEBUG
                catch (Exception ex)
                {
                    return;
                }
#endif
                finally
                {
                    IsInfoDownloading = false;
                }
            });
        }
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        public Task Download()
        {
            return Task.Run(() =>
            {
                if (!IsInfoDownloaded)
                {
                    return;
                }
                if (IsDownloaded || IsDownloading)
                {
                    return;
                }
                try
                {
                    using (var client = getWebClient())
                    {
                        IsDownloading = true;
                        if (!jsonHash.Contains(Hash)/* && IsNewToday*/)
                        {
                            var data = client.DownloadDataAsTask(DownloadUrl, tokenSource.Token).Result;
                            Debug.WriteLine($"Data fetched: {LocaleName}-{data.GetHashCode()}");
                            if (!imageHash.Contains(data) && !containsData(data))
                            {
                                ImageProcesser.SaveToFile(data, FullFileName);
                                Debug.WriteLine($"Downloaded: {LocaleName}");
                                DownloadedCount++;
                                imageHash.Add(data);
                                jsonHash.Add(Hash);
                            }
                        }
                    }
                }
                catch (AggregateException ex)
                when (ex.InnerException is WebException)
                {
                    return;
                }
                catch (Exception ex)
                when (ex is OperationCanceledException || ex is TaskCanceledException)
                {
                    return;
                }
#if !DEBUG
                catch (Exception ex)
                {
                    return;
                }
#endif
                finally
                {
                    IsDownloading = false;
                }
            });
        }
        public void CancelDownload() => tokenSource?.Cancel();
    }
}
