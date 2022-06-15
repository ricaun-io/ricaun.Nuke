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
        public static async Task DownloadFileTaskAsync(this System.Net.Http.HttpClient client, Uri address, string fileName)
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
