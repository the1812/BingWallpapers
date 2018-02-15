using Ace.Files.Json;
using BingWallpapers.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BingWallpapers.Model
{
    static class Settings
    {
        internal const string FileName = "settings.json";
        public static void Load()
        {
            var file = new JsonFile(FileName);
            if (file.Exists)
            {
                try
                {
                    file.Load();
                    var content = file.ObjectContent;
                    if (content.ContainsName(nameof(DownloadPath)))
                    {
                        DownloadPath = content[nameof(DownloadPath)].StringValue;
                    }
                    if (content.ContainsName(nameof(FileNameFormat)))
                    {
                        FileNameFormat = content[nameof(FileNameFormat)].StringValue;
                    }
                }
                catch
                {
#if DEBUG
                    throw;
#endif
                }
            }
        }
        public static void Save()
        {
            var file = new JsonFile(FileName)
            {
                ObjectContent = new JsonObject
                {
                    new JsonProperty(nameof(DownloadPath), DownloadPath),
                    new JsonProperty(nameof(FileNameFormat), FileNameFormat),
                },
            };
            try
            {
                file.Save();
            }
            catch (UnauthorizedAccessException ex)
            {
                var language = new MainLanguage();
                var title = language[MainLanguage.Keys.SettingsSaveFailedTitle];
                var message = language[MainLanguage.Keys.SettingsSaveFailedMessage] + Environment.NewLine + ex.Message;
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
#if !DEBUG
            catch (Exception)
            {

            }
#endif
        }
        public static string DownloadPath { get; set; } = "";
        public static string FileNameFormat { get; set; } = $"{FormatYear}-{FormatMonth}-{FormatDay}-{FormatLocale}";
        public const string FormatYear = @"${Year}";
        public const string FormatMonth = @"${Month}";
        public const string FormatDay = @"${Day}";
        public const string FormatLocale = @"${Locale}";
    }
}
