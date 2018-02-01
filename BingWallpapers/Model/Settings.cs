using Ace.Files.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                file.Load();
                var content = file.ObjectContent;
                if (content.ContainsName(nameof(DownloadPath)))
                {
                    DownloadPath = content[nameof(DownloadPath)].StringValue;
                }
                else
                {
                    fillDefault();
                }
            }
            else
            {
                fillDefault();
            }
        }
        public static void Save()
        {
            var file = new JsonFile(FileName)
            {
                ObjectContent = new JsonObject
                {
                    new JsonProperty(nameof(DownloadPath), DownloadPath),
                },
            };
            try
            {
                file.Save();
            }
            catch (UnauthorizedAccessException)
            {

            }
#if !DEBUG
            catch (Exception)
            {

            }
#endif
        }
        private static void fillDefault()
        {

            DownloadPath = "";
        }
        public static string DownloadPath { get; set; } = "";
    }
}
