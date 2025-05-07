using System.IO;

namespace BloodShadow.Core.HttpRequests
{
    public static class HttpRequests
    {
        public static async Task Download(string uri, string savePath)
        {
            using HttpClient hc = new();
            using HttpResponseMessage response = await hc.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();
            using Stream stream = await response.Content.ReadAsStreamAsync();
            if (!Path.Exists(savePath)) { Directory.CreateDirectory(Path.GetDirectoryName(savePath) ?? ""); }
            using var fs = File.OpenWrite(savePath);
            await stream.CopyToAsync(fs);
        }
    }
}
