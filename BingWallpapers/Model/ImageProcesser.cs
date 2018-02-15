using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpapers.Model
{
    static class ImageProcesser
    {
        public static byte[] LoadFromFile(string fileName)
        {
            using (var stream = File.OpenRead(fileName))
            {
                var data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);
                return data;
            }
        }
        public static void SaveToFile(byte[] data, Wallpaper wallpaper)
        {
            using (var stream = File.OpenWrite(wallpaper.FullFileName))
            {
                stream.Write(data, 0, data.Length);
            }
        }
    }
}
