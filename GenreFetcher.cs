using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Trackify
{
    public class GenreFetcher
    {
        private static readonly HttpClient client = new HttpClient();
        private const string API_KEY = "2c0af09383ef18f74e4c6b022a036298";

        public async Task<string> GetGenreAsync(string artist)
        {
            try
            {
                string url =
                    $"https://ws.audioscrobbler.com/2.0/?method=artist.getinfo&artist={Uri.EscapeDataString(artist)}&api_key={API_KEY}&format=json";

                var response = await client.GetStringAsync(url);

                using var doc = JsonDocument.Parse(response);

                var tags = doc.RootElement
                    .GetProperty("artist")
                    .GetProperty("tags")
                    .GetProperty("tag");

                    if (tags.GetArrayLength() > 0)
                    {
                        return tags[0].GetProperty("name").GetString() ?? "Unknown";
                    }

                    return "Unknown";
            }
            catch
            {
                return "Unknown";
            }
        }
    }
}