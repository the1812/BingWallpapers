using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpapers.Model
{
    static class FileNameChecker
    {
        public static bool IsValid(string fileName) 
            => (fileName?.IndexOfAny(Path.GetInvalidFileNameChars()) ?? 0) < 0;
    }
}
