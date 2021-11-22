﻿using System.Net.Http;
using System.Text;

namespace Common.Utilities
{
    public class HttpClientUtility
    {
        private readonly HttpClient _client = new HttpClient();
        public HttpResponseMessage SendJson(string json, string url, string method)
        {
            var httpMethod = new HttpMethod(method.ToUpper());
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(httpMethod, url)
            {
                Content = content
            };
            var task = _client.SendAsync(request);
            task.Wait();
            return task.Result;
        }
    }
}
