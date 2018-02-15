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
using System.Collections.Concurrent;
using System.Globalization;

namespace BingWallpapers.Model
{
    sealed class Wallpaper
    {
        private static ConcurrentBag<string> jsonHashs = new ConcurrentBag<string>();
        private static ConcurrentBag<byte[]> imageDatas = new ConcurrentBag<byte[]>();
        public static void ResetDownloadedInfo()
        {
            void clearBag<T>(ConcurrentBag<T> bag)
            {
                //Though not atomic, ResetHash() is called by only one thread.
                while (!bag.IsEmpty)
                {
                    bag.TryTake(out var item);
                }
            };
            clearBag(jsonHashs);
            clearBag(imageDatas);
            DownloadedCount = 0;
            foreach (var file in new DirectoryInfo(Settings.DownloadPath).EnumerateFiles())
            {
                imageDatas.Add(ImageProcesser.LoadFromFile(file.FullName));
            }
        }
        public static int DownloadedCount { get; private set; } = 0;
        private static bool containsData(byte[] data)
        {
            foreach (var image in imageDatas)
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
        //public string FriendlyLocaleName => new CultureInfo(LocaleName).DisplayName;
        public string LocaleName { get; private set; } = "Unknown";
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
        public DownloadSpeed DownloadSpeed { get; private set; } = new DownloadSpeed(0);
        private WebClient getWebClient()
        {
            var webClient = new WebClient
            {
                Encoding = Encoding.UTF8,
            };
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
                        //client.DownloadProgressChanged += DownloadSpeed.GetProgressHandler();
                        IsInfoDownloading = true;
                        var info = client.DownloadStringAsTask(InfoUrl, tokenSource.Token).Result;
                        var parseResult = JsonObject.TryParse(info, out var json);
                        Debug.Assert(parseResult);
#if !DEBUG
                        if (!parseResult)
                        {
                            return;
                        }
#endif
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
                catch (Exception)
                {
                    return;
                }
#endif
                finally
                {
                    IsInfoDownloading = false;
                    DownloadSpeed.Reset();
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
                        //client.DownloadProgressChanged += DownloadSpeed.GetProgressHandler();
                        IsDownloading = true;
                        if (!jsonHashs.Contains(Hash)/* && IsNewToday*/)
                        {
                            var data = client.DownloadDataAsTask(DownloadUrl, tokenSource.Token).Result;
                            Debug.WriteLine($"Data fetched: {LocaleName}-{data.GetHashCode()}");
                            if (!imageDatas.Contains(data) && !containsData(data)/* byte-by-byte compare */)
                            {
                                imageDatas.Add(data);
                                jsonHashs.Add(Hash);
                                ImageProcesser.SaveToFile(data, this);
                                Debug.WriteLine($"Downloaded: {LocaleName}");
                                DownloadedCount++;
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
                catch (Exception)
                {
                    return;
                }
#endif
                finally
                {
                    IsDownloading = false;
                    DownloadSpeed.Reset();
                }
            });
        }
        public void CancelDownload() => tokenSource?.Cancel();
    }
}
