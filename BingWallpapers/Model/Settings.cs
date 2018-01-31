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
                if (content.ContainsName(nameof(DownloadPath)) &&
                    content.ContainsName(nameof(RestrictedNetwork)))
                {
                    DownloadPath = content[nameof(DownloadPath)].StringValue;
                    RestrictedNetwork = content[nameof(RestrictedNetwork)].BooleanValue.Value;
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
                    new JsonProperty(nameof(RestrictedNetwork), RestrictedNetwork),
                },
            };
            file.Save();
        }
        private static void fillDefault()
        {
            DownloadPath = "";
            RestrictedNetwork = false;
        }
        public static string DownloadPath { get; set; } = "";
        public static bool RestrictedNetwork { get; set; } = false;
    }
}
