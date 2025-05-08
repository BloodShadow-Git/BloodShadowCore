using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace BloodShadow.Core.HttpRequests
{
    public static class HttpRequests
    {
        public static Task DownloadFile(string uri, string savePath) => DownloadFile(new Uri(uri), savePath);
        public static async Task DownloadFile(Uri uri, string savePath)
        {
            if (uri.IsFile) { File.Copy(uri.AbsolutePath, savePath); }
            else
            {
                using HttpClient hc = new HttpClient();
                using HttpResponseMessage response = await hc.GetAsync(uri.AbsoluteUri, HttpCompletionOption.ResponseHeadersRead);

                if (response.IsSuccessStatusCode)
                {
                    FileInfo fi = new FileInfo(savePath);
                    if (!fi.Exists) { Directory.CreateDirectory(fi.DirectoryName ?? ""); }
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    await File.WriteAllBytesAsync(savePath, buffer);
                }
            }
        }
    }
}
