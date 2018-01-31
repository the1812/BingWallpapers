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
        }
        public static int HashCount => imageHash.Count;
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
        public string FriendlyLocaleName { get; private set; }
        public string LocaleName { get; private set; }
        public string FileName => $"{Date.ToShortDateString()}-{LocaleName}.jpg";
        public string FullFileName => $"{Settings.DownloadPath.Backslash()}{FileName}";
        private string InfoUrl => $"https://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt={LocaleName}";
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
                    using (var client = new WebClient())
                    {
                        IsInfoDownloading = true;
                        client.Encoding = Encoding.UTF8;

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
                        if (IsDownloaded)
                        {
                            jsonHash.Add(Hash);
                            var data = ImageProcesser.LoadFromFile(FullFileName);
                            imageHash.Add(data);
                            Debug.WriteLine($"Already downloaded: {data.GetHashCode()}");
                        }
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
                    using (var client = new WebClient())
                    {
                        IsDownloading = true;
                        client.Encoding = Encoding.UTF8;
                        if (!jsonHash.Contains(Hash) && IsNewToday)
                        {
                            var data = client.DownloadDataAsTask(DownloadUrl, tokenSource.Token).Result;
                            Debug.WriteLine($"Data fetched: {data.GetHashCode()}");
                            if (!imageHash.Contains(data) && !containsData(data))
                            {
                                ImageProcesser.SaveToFile(data, FullFileName);
                                Debug.WriteLine($"Downloaded: {LocaleName}");
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
                finally
                {
                    IsDownloading = false;
                }
            });
        }
        public void CancelDownload() => tokenSource?.Cancel();
    }
}
