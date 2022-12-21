using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace ricaun.Nuke.Extensions
{
    /// <summary>
    /// HttpClientExtension
    /// </summary>
    public static class HttpClientExtension
    {
        /// <summary>
        /// Download retry number error
        /// </summary>
        private const int DOWNLOAD_NUMBER_RETRY = 5;

        /// <summary>
        /// Download retry millis delay/sleep
        /// </summary>
        private const int DOWNLOAD_DELAY_RETRY = 5000;

        /// <summary>
        /// Download File Retry if error
        /// </summary>
        /// <param name="address"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool DownloadFileRetry(string address, string fileName)
        {
            for (int i = DOWNLOAD_NUMBER_RETRY; i >= 0; i--)
            {
                try
                {
                    DownloadFile(address, fileName);
                    return true;
                }
                catch (Exception ex)
                {
                    Serilog.Log.Warning($"DownloadFileRetry: {ex.Message}");
                    if (i == 0) throw;
                    System.Threading.Thread.Sleep(DOWNLOAD_DELAY_RETRY);
                }
            }
            return false;
        }

        /// <summary>
        /// Download File
        /// </summary>
        /// <param name="address"></param>
        /// <param name="fileName"></param>
        public static void DownloadFile(string address, string fileName)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", Assembly.GetCallingAssembly().GetName().Name);
                client.DownloadFile(address, fileName);
            }
        }

        /// <summary>
        /// DownloadFile
        /// </summary>
        /// <param name="client"></param>
        /// <param name="address"></param>
        /// <param name="fileName"></param>
        public static void DownloadFile(this System.Net.Http.HttpClient client, string address, string fileName)
        {
            if (File.Exists(fileName))
            {
                Serilog.Log.Information($"DownloadFile: Exists: {fileName}");
                return;
            }

            Serilog.Log.Information($"DownloadFile: {fileName}");

            var uri = new Uri(address);
            var task = Task.Run(async () =>
            {
                await client.DownloadFileTaskAsync(uri, fileName);
            });
            task.GetAwaiter().GetResult();
        }

        /// <summary>
        /// DownloadFileTaskAsync
        /// </summary>
        /// <param name="client"></param>
        /// <param name="address"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static async Task DownloadFileTaskAsync(this System.Net.Http.HttpClient client, Uri address, string fileName)
        {
            using (var s = await client.GetStreamAsync(address))
            {
                using (var fs = new FileStream(fileName, FileMode.CreateNew))
                {
                    await s.CopyToAsync(fs);
                }
            }
        }
    }
}
