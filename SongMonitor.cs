using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Media.Control;

namespace Trackify
{
    public class SongMonitor
    {
        private GlobalSystemMediaTransportControlsSessionManager? _manager;

        public async void Start(Func<string, string, byte[]?, Task> onSongChanged)
        {
            _manager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();

            string lastTitle = "";
            string lastArtist = "";

            while (true)
            {
                var session = _manager.GetCurrentSession();

                if (session != null)
                {
                    var properties = await session.TryGetMediaPropertiesAsync();

                    byte[]? imageBytes = null;

                    if (properties.Thumbnail != null)
                    {
                        using var stream = await properties.Thumbnail.OpenReadAsync();
                        using var input = stream.AsStreamForRead();
                        using var ms = new MemoryStream();

                        await input.CopyToAsync(ms);

                        imageBytes = ms.ToArray();
                    }

                    string title = properties.Title ?? "Unknown";
                    string artist = properties.Artist ?? "Unknown";

                    if (title != lastTitle || artist != lastArtist)
                    {
                        lastTitle = title;
                        lastArtist = artist;

                        await onSongChanged(title, artist, imageBytes);
                    }
                }

                await Task.Delay(2000);
            }
        }
    }
}