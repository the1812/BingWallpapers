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
using System.Windows.Media.Imaging;
using GdiBitmap = System.Drawing.Bitmap;
using Ace.Files;

namespace BingWallpapers.Model
{
    sealed class Wallpaper
    {
        private static byte[] testData;
        private static ConcurrentBag<string> jsonHashs = new ConcurrentBag<string>();
        private static ConcurrentBag<byte[]> imageDatas = new ConcurrentBag<byte[]>();
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
        public static Task ResetDownloadedInfo()
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
            return Task.Run(() =>
            {
                foreach (var file in new DirectoryInfo(Settings.DownloadPath).EnumerateFiles())
                {
                    var data = ImageProcesser.ExtractPixelData(ImageProcesser.LoadFromFile(file.FullName));
                    imageDatas.Add(data);
                    if (file.Name.Contains("2018-02-17-bs-ba"))
                    {
                        testData = data;
                    }
                }
            });
        }
        public static int DownloadedCount { get; private set; } = 0;

        private CancellationTokenSource tokenSource = new CancellationTokenSource();

        public Wallpaper(string locale, string friendlyName)
        {
            Info.LocaleName = locale;
            Info.FriendlyLocaleName = friendlyName;
        }

        public WallpaperInfo Info { get; private set; } = new WallpaperInfo();

        public bool IsInfoDownloaded { get; private set; }
        public bool IsInfoDownloading { get; private set; }
        public bool IsDownloaded => File.Exists(Info.FullFileName);
        public bool IsDownloading { get; private set; }

        private WebClient getWebClient()
        {
            var webClient = new WebClient
            {
                Encoding = Encoding.UTF8,
            };
            return webClient;
        }
        //public DownloadSpeed DownloadSpeed { get; private set; } = new DownloadSpeed(0);
        public void CancelDownload() => tokenSource?.Cancel();
        
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
                        var info = client.DownloadStringAsTask(Info.InfoUrl, tokenSource.Token).Result;
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
                        Info.Date = new DateTime(
                            timeString.Substring(0, 4).ToInt32(),
                            timeString.Substring(4, 2).ToInt32(),
                            timeString.Substring(6, 2).ToInt32(),
                            timeString.Substring(8, 2).ToInt32(),
                            timeString.Substring(10, 2).ToInt32(),
                            0);
                        Info.DownloadUrl = $"https://www.bing.com{json["url"].StringValue}";
                        var copyright = json["copyright"].StringValue;
                        var match = copyright.Match(@"(.*?) \((©.*?)\)");
                        if (match.Success)
                        {
                            Info.Title = match.Groups[1].Value;
                            Info.Copyright = match.Groups[2].Value;
                        }
                        else
                        {
                            Info.Title = "";
                            Info.Copyright = copyright;
                        }
                        Info.Hash = json["hsh"].StringValue;
                        Debug.WriteLineIf(IsDownloaded, $"Already downloaded: {Info.LocaleName}-{Info.Hash}");
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
                    //DownloadSpeed.Reset();
                }
            });
        }
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
                        if (!jsonHashs.Contains(Info.Hash))
                        {
                            var rawData = client.DownloadDataAsTask(Info.DownloadUrl, tokenSource.Token).Result;
                            rawData = ImageProcesser.AddMetadata(rawData, Info);
                            var pixelData = ImageProcesser.ExtractPixelData(rawData);
                            Debug.WriteLine($"Data fetched: {Info.LocaleName}-{rawData.GetHashCode()}");
                            if (!imageDatas.Contains(pixelData) && !containsData(pixelData))
                            {
                                imageDatas.Add(pixelData);
                                jsonHashs.Add(Info.Hash);
                                ImageProcesser.SaveToFile(rawData, Info);
                                Debug.WriteLine($"Downloaded: {Info.LocaleName}");
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
                    //DownloadSpeed.Reset();
                }
            });
        }
    }
}
