using Ace.Files.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpapers.Model
{
    static class Locales
    {
        internal const string FileName = "locales.json";
        public static Dictionary<string, string> Dictionary => localeStrings;
        public static List<Wallpaper> Wallpapers 
            => new List<Wallpaper>(localeStrings.Select(p => new Wallpaper(p.Value, p.Key)));
        private static Dictionary<string, string> localeStrings;
        static Locales()
        {
            var file = new JsonFile(FileName).Load();
            var datas = from prop in file.ObjectContent
                       select new KeyValuePair<string, string>(prop.Name, prop.StringValue);
            localeStrings = new Dictionary<string, string>();
            foreach (var data in datas)
            {
                localeStrings.Add(data.Key, data.Value);
            }
        }
    }
}
