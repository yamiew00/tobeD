using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ExamKeeper.JerryH.Jutils
{
    /// <summary>
    /// 用來發API的工具
    /// </summary>
    public class APISender
    {
        /// <summary>
        /// 這個可做成單例
        /// </summary>
        private readonly HttpClient HttpClient = new HttpClient();

        private readonly HttpRequestMessage HttpRequestMessage;

        private string ContentType = "application/json";

        /// <summary>
        /// 私有建構子
        /// </summary>
        private APISender()
        {
            HttpRequestMessage = new HttpRequestMessage();
        }

        private async Task<T> SendAsync<T>()
        {
            var response = await HttpClient.SendAsync(HttpRequestMessage);
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseString);
            }

            return default;
        }

        public async Task<T> GetAsync<T>()
        {
            HttpRequestMessage.Method = HttpMethod.Get;
            return await SendAsync<T>();
        }

        public async Task<T> PostAsync<T>()
        {
            HttpRequestMessage.Method = HttpMethod.Post;
            return await SendAsync<T>();
        }

        public static APISender Create()
        {
            return new APISender();
        }

        public APISender AddUri(string uri)
        {
            HttpRequestMessage.RequestUri = new Uri(uri);
            return this;
        }

        /// <summary>
        /// 新增contentType。有bug
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        //public APISender AddContentType(string contentType)
        //{
        //    ContentType = contentType;
        //    if (HttpRequestMessage.Content == null)
        //    {
        //        return this;
        //    }

        //    HttpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
        //    return this;
        //}

        public APISender AddAuthorization(string value)
        {
            HttpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue(value);
            return this;
        }

        public APISender AddRequestBody(object body)
        {
            var toJson = JsonConvert.SerializeObject(body);
            HttpRequestMessage.Content = new StringContent(toJson);

            if (!string.IsNullOrEmpty(ContentType))
            {
                HttpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(ContentType);
            }
            return this;
        }
    }
}
