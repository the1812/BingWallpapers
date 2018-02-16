using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

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
        public static void SaveToFile(byte[] data, WallpaperInfo info)
        {
            using (var stream = File.OpenWrite(info.FullFileName))
            {
                //data = SetMetadata(data, info);
                stream.Write(data, 0, data.Length);
            }
        }
        public static byte[] AddMetadata(byte[] data, WallpaperInfo info)
        {
            using (var stream = new MemoryStream(data))
            {
                var decoder = new JpegBitmapDecoder(
                    stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                var metadata = decoder.Metadata;
                if (metadata is null)
                {
                    metadata = new BitmapMetadata("jpg");
                }
                metadata.Copyright = info.Copyright;
                metadata.Title = info.Title;

                var frame = decoder.Frames[0];
                var encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(frame, frame.Thumbnail, metadata, frame.ColorContexts));

                using (var outputStream = new MemoryStream())
                {
                    encoder.Save(outputStream);
                    return outputStream.ToArray();
                }
            }
        }
        public static byte[] RemoveMetadata(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                var decoder = new JpegBitmapDecoder(
                    stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                var metadata = decoder.Metadata;
                if (metadata is null)
                {
                    metadata = new BitmapMetadata("jpg");
                }
                metadata.Copyright = "";
                metadata.Title = "";

                var frame = decoder.Frames[0];
                var encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(frame, frame.Thumbnail, metadata, frame.ColorContexts));

                using (var outputStream = new MemoryStream())
                {
                    encoder.Save(outputStream);
                    return outputStream.ToArray();
                }
            }
        }
    }
}
