using Ace.Files.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames()
                .FirstOrDefault(name => name.Contains(FileName));
            Debug.Assert(resourceName != null);
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                var jsonData = new byte[stream.Length];
                stream.Read(jsonData, 0, jsonData.Length);
                var jsonString = Encoding.UTF8.GetString(jsonData);
                
                var datas = from prop in JsonObject.Parse(jsonString)
                            select new KeyValuePair<string, string>(prop.Name, prop.StringValue);
                localeStrings = new Dictionary<string, string>();
                foreach (var data in datas)
                {
                    localeStrings.Add(data.Key, data.Value);
                }
            }
        }
    }
}
