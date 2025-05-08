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
                using HttpClient hc = new();
                using HttpResponseMessage response = await hc.GetAsync(uri.AbsoluteUri, HttpCompletionOption.ResponseHeadersRead);

                if (response.IsSuccessStatusCode)
                {
                    if (!Path.Exists(savePath)) { Directory.CreateDirectory(Path.GetDirectoryName(savePath) ?? ""); }
                    byte[] buffer = await response.Content.ReadAsByteArrayAsync();
                    await File.WriteAllBytesAsync(savePath, buffer);
                }
            }
        }
    }
}
