using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Ace;
using System.Diagnostics;

namespace BingWallpapers.Model
{
    sealed class DownloadSpeed
    {
        private double bytesPerSecond;
        public DownloadSpeed(double bytesPerSecond) => this.bytesPerSecond = bytesPerSecond;
        public string Speed
        {
            get
            {
                var dataSize = new DataSize((long) bytesPerSecond);
                return $"{dataSize}/s";
            }
        }
        public double BytesPerSecond => bytesPerSecond;
        public void Reset() => bytesPerSecond = 0;
        public DownloadProgressChangedEventHandler GetProgressHandler()
        {
            var lastTime = DateTime.Now;
            var lastBytes = 0L;
            return (s, e) =>
            {
                if (lastBytes == 0L)
                {
                    lastTime = DateTime.Now;
                    lastBytes = e.BytesReceived;
                    return;
                }

                var now = DateTime.Now;
                var timeSpan = lastTime - now;
                var newBytes = e.BytesReceived - lastBytes;
                bytesPerSecond = newBytes / timeSpan.TotalSeconds;
                Debug.WriteLineIf(bytesPerSecond != 0, bytesPerSecond);

                lastBytes = e.BytesReceived;
                lastTime = now;
            };
        }
    }
}
