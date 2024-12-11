using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ricaun.Nuke.IO;

public static class HttpAuthTasks
{
    #region HttpPost
    public static async Task<string> HttpPostFileAsync(
        string uri,
        string filePath,
        string authorization = null,
        Dictionary<string, string> formData = null,
        string fileStreamContentName = null,
        Configure<HttpClient> clientConfigurator = null,
        Action<HttpRequestHeaders> headerConfigurator = null)
    {
        var content = CreateFormDataContent(filePath, formData, fileStreamContentName);
        return await HttpPostAsync(uri, content, authorization, clientConfigurator, headerConfigurator);
    }

    public static string HttpPostFile(
        string uri,
        string filePath,
        string authorization = null,
        Dictionary<string, string> formData = null,
        string fileStreamContentName = null,
        Configure<HttpClient> clientConfigurator = null,
        Action<HttpRequestHeaders> headerConfigurator = null)
    {
        return HttpPostFileAsync(uri, filePath, authorization, formData, fileStreamContentName, clientConfigurator, headerConfigurator).GetAwaiter().GetResult();
    }

    public static async Task<string> HttpPostAsync(
        string uri,
        object content,
        string authorization = null,
        Configure<HttpClient> clientConfigurator = null,
        Action<HttpRequestHeaders> headerConfigurator = null)
    {
        return await HttpPostAsync(uri, JsonHttpContent(content), authorization, clientConfigurator, headerConfigurator);
    }

    public static string HttpPost(
        string uri,
        object content,
        string authorization = null,
        Configure<HttpClient> clientConfigurator = null,
        Action<HttpRequestHeaders> headerConfigurator = null)
    {
        return HttpPostAsync(uri, content, authorization, clientConfigurator, headerConfigurator).GetAwaiter().GetResult();
    }

    public static async Task<string> HttpPostAsync(
        string uri,
        HttpContent httpContent = null,
        string authorization = null,
        Configure<HttpClient> clientConfigurator = null,
        Action<HttpRequestHeaders> headerConfigurator = null)
    {
        var httpClient = CreateHttpClient(authorization, clientConfigurator, headerConfigurator);
        return await httpClient.PostAsync(uri, httpContent).Result.Content.ReadAsStringAsync();
    }
    public static string HttpPost(
        string uri,
        HttpContent httpContent = null,
        string authorization = null,
        Configure<HttpClient> clientConfigurator = null,
        Action<HttpRequestHeaders> headerConfigurator = null)
    {
        return HttpPostAsync(uri, httpContent, authorization, clientConfigurator, headerConfigurator).GetAwaiter().GetResult();
    }
    #endregion

    #region HttpPut
    public static async Task<string> HttpPutFileAsync(
        string uri,
        string filePath,
        string authorization = null,
        Dictionary<string, string> formData = null,
        string fileStreamContentName = null,
        Configure<HttpClient> clientConfigurator = null,
        Action<HttpRequestHeaders> headerConfigurator = null)
    {
        var content = CreateFormDataContent(filePath, formData, fileStreamContentName);
        return await HttpPostAsync(uri, content, authorization, clientConfigurator, headerConfigurator);
    }

    public static string HttpPutFile(
        string uri,
        string filePath,
        string authorization = null,
        Dictionary<string, string> formData = null,
        string fileStreamContentName = null,
        Configure<HttpClient> clientConfigurator = null,
        Action<HttpRequestHeaders> headerConfigurator = null)
    {
        return HttpPutFileAsync(uri, filePath, authorization, formData, fileStreamContentName, clientConfigurator, headerConfigurator).GetAwaiter().GetResult();
    }

    public static async Task<string> HttpPutAsync(
        string uri,
        object content,
        string authorization = null,
        Configure<HttpClient> clientConfigurator = null,
        Action<HttpRequestHeaders> headerConfigurator = null)
    {
        return await HttpPutAsync(uri, JsonHttpContent(content), authorization, clientConfigurator, headerConfigurator);
    }

    public static string HttpPut(
        string uri,
        object content,
        string authorization = null,
        Configure<HttpClient> clientConfigurator = null,
        Action<HttpRequestHeaders> headerConfigurator = null)
    {
        return HttpPutAsync(uri, content, authorization, clientConfigurator, headerConfigurator).GetAwaiter().GetResult();
    }
    public static async Task<string> HttpPutAsync(
        string uri,
        HttpContent httpContent = null,
        string authorization = null,
        Configure<HttpClient> clientConfigurator = null,
        Action<HttpRequestHeaders> headerConfigurator = null)
    {
        var httpClient = CreateHttpClient(authorization, clientConfigurator, headerConfigurator);
        return await httpClient.PutAsync(uri, httpContent).Result.Content.ReadAsStringAsync();
    }
    public static string HttpPut(
        string uri,
        HttpContent httpContent = null,
        string authorization = null,
        Configure<HttpClient> clientConfigurator = null,
        Action<HttpRequestHeaders> headerConfigurator = null)
    {
        return HttpPutAsync(uri, httpContent, authorization, clientConfigurator, headerConfigurator).GetAwaiter().GetResult();
    }
    #endregion

    #region HttpPatch
    public static async Task<string> HttpPatchFileAsync(
        string uri,
        string filePath,
        string authorization = null,
        Dictionary<string, string> formData = null,
        string fileStreamContentName = null,
        Configure<HttpClient> clientConfigurator = null,
        Action<HttpRequestHeaders> headerConfigurator = null)
    {
        var content = CreateFormDataContent(filePath, formData, fileStreamContentName);
        return await HttpPostAsync(uri, content, authorization, clientConfigurator, headerConfigurator);
    }

    public static string HttpPatchFile(
        string uri,
        string filePath,
        string authorization = null,
        Dictionary<string, string> formData = null,
        string fileStreamContentName = null,
        Configure<HttpClient> clientConfigurator = null,
        Action<HttpRequestHeaders> headerConfigurator = null)
    {
        return HttpPatchFileAsync(uri, filePath, authorization, formData, fileStreamContentName, clientConfigurator, headerConfigurator).GetAwaiter().GetResult();
    }

    public static async Task<string> HttpPatchAsync(
        string uri,
        object content,
        string authorization = null,
        Configure<HttpClient> clientConfigurator = null,
        Action<HttpRequestHeaders> headerConfigurator = null)
    {
        return await HttpPatchAsync(uri, JsonHttpContent(content), authorization, clientConfigurator, headerConfigurator);
    }

    public static string HttpPatch(
        string uri,
        object content,
        string authorization = null,
        Configure<HttpClient> clientConfigurator = null,
        Action<HttpRequestHeaders> headerConfigurator = null)
    {
        return HttpPatchAsync(uri, content, authorization, clientConfigurator, headerConfigurator).GetAwaiter().GetResult();
    }
    public static async Task<string> HttpPatchAsync(
        string uri,
        HttpContent httpContent = null,
        string authorization = null,
        Configure<HttpClient> clientConfigurator = null,
        Action<HttpRequestHeaders> headerConfigurator = null)
    {
        var httpClient = CreateHttpClient(authorization, clientConfigurator, headerConfigurator);
        return await httpClient.PatchAsync(uri, httpContent).Result.Content.ReadAsStringAsync();
    }
    public static string HttpPatch(
        string uri,
        HttpContent httpContent = null,
        string authorization = null,
        Configure<HttpClient> clientConfigurator = null,
        Action<HttpRequestHeaders> headerConfigurator = null)
    {
        return HttpPatchAsync(uri, httpContent, authorization, clientConfigurator, headerConfigurator).GetAwaiter().GetResult();
    }
    #endregion

    #region HttpGet
    public static void HttpGetFile(
        string uri,
        string path,
        string authorization = null,
        FileMode mode = FileMode.Create,
        Configure<HttpClient> clientConfigurator = null,
        Action<HttpRequestHeaders> headerConfigurator = null)
    {
        HttpGetFileAsync(uri, path, authorization, mode, clientConfigurator, headerConfigurator).GetAwaiter().GetResult();
    }
    public static async Task HttpGetFileAsync(
        string uri,
        AbsolutePath path,
        string authorization = null,
        FileMode mode = FileMode.Create,
        Configure<HttpClient> clientConfigurator = null,
        Action<HttpRequestHeaders> headerConfigurator = null)
    {
        var httpClient = CreateHttpClient(authorization, clientConfigurator, headerConfigurator);
        var response = await httpClient.GetAsync(uri);
        Assert.True(response.IsSuccessStatusCode, $"{response.ReasonPhrase}: {uri}");

        path.Parent.CreateDirectory();
        await using var fileStream = File.Open(path, mode);
        await response.Content.CopyToAsync(fileStream);
    }
    public static async Task<string> HttpGetAsync(
        string uri,
        string authorization = null,
        Configure<HttpClient> clientConfigurator = null,
        Action<HttpRequestHeaders> headerConfigurator = null)
    {
        var httpClient = CreateHttpClient(authorization, clientConfigurator, headerConfigurator);
        return await httpClient.GetAsync(uri).Result.Content.ReadAsStringAsync();
    }
    public static string HttpGet(
        string uri,
        string authorization = null,
        Configure<HttpClient> clientConfigurator = null,
        Action<HttpRequestHeaders> headerConfigurator = null)
    {
        return HttpGetAsync(uri, authorization, clientConfigurator, headerConfigurator).GetAwaiter().GetResult();
    }
    #endregion

    #region HttpDelete
    public static async Task<string> HttpDeleteAsync(
        string uri,
        string authorization = null,
        Configure<HttpClient> clientConfigurator = null,
        Action<HttpRequestHeaders> headerConfigurator = null)
    {
        var httpClient = CreateHttpClient(authorization, clientConfigurator, headerConfigurator);
        return await httpClient.DeleteAsync(uri).Result.Content.ReadAsStringAsync();
    }
    public static string HttpDelete(
        string uri,
        string authorization = null,
        Configure<HttpClient> clientConfigurator = null,
        Action<HttpRequestHeaders> headerConfigurator = null)
    {
        return HttpDeleteAsync(uri, authorization, clientConfigurator, headerConfigurator).GetAwaiter().GetResult();
    }
    #endregion

    #region HttpClient
    public static TimeSpan DefaultTimeout = TimeSpan.FromSeconds(10);

    private static HttpClient CreateHttpClient(
        string authorization = null,
        Configure<HttpClient> clientConfigurator = null,
        Action<HttpRequestHeaders> headerConfigurator = null)
    {
        var httpClient = new HttpClient { Timeout = DefaultTimeout };
        SetBearerAuthorization(httpClient, authorization);
        clientConfigurator?.Invoke(httpClient);
        headerConfigurator?.Invoke(httpClient.DefaultRequestHeaders);
        return httpClient;
    }
    private static void SetBearerAuthorization(HttpClient httpClient, string authorization = null)
    {
        const string HeaderAuthorization = "Authorization";
        if (!string.IsNullOrEmpty(authorization))
        {
            httpClient.DefaultRequestHeaders.Add(HeaderAuthorization, $"Bearer {authorization}");
        }
    }

    private static HttpContent JsonHttpContent(object content)
    {
        if (content is null)
            content = string.Empty;

        HttpContent httpContent = null;
        if (content is string stringContent)
        {
            httpContent = new StringContent(stringContent);
        }
        else if (content is HttpContent httpContentTo)
        {
            httpContent = httpContentTo;
        }
        else
        {
            const string MediaTypeJson = "application/json";
            httpContent = new StringContent(content.ToJson(), System.Text.Encoding.UTF8, MediaTypeJson);
        }

        return httpContent;
    }

    internal static MultipartFormDataContent CreateFormDataContent(string filePath, Dictionary<string, string> formData = null, string fileStreamContentName = null)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("File not found.", filePath);

        var content = new MultipartFormDataContent();

        if (formData is not null)
        {
            foreach (KeyValuePair<string, string> vp in formData)
            {
                content.Add(new StringContent(vp.Value), vp.Key);
            }
        }

        if (string.IsNullOrEmpty(fileStreamContentName)) fileStreamContentName = "file";

        var streamContent = new StreamContent(new FileStream(filePath, FileMode.Open));
        content.Add(streamContent, fileStreamContentName, Path.GetFileName(filePath));

        return content;
    }
    #endregion

}
internal static class JsonExtension
{
    internal static string ToJson(this object obj)
    {
        if (obj is string t)
            return t;

        return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
    }
    internal static T FromJson<T>(this string json)
    {
        if (json is T t)
            return t;

        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
    }
}