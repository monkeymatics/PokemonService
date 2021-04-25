using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PokemonCore.Infrastructure
{
    public class HttpClient : IHttpClient
    {
        private readonly System.Net.Http.HttpClient _client;

        public HttpClient() => _client = new System.Net.Http.HttpClient();

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await MakeRequest("GET", url);
        }

        public async Task<ReturnType<T>> GetAsync<T>(string url)
        {
            var response = await GetAsync(url);

            if (response.StatusCode == HttpStatusCode.OK)
                return new ReturnType<T>(JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync()), response);
            else
                return new ReturnType<T>(response);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent requestContent)
        {
            return await MakeRequest("POST", url, requestContent);
        }

        public async Task<ReturnType<T>> PostAsync<T>(string url, HttpContent requestContent)
        {
            var response = await PostAsync(url, requestContent);
            if (response.StatusCode == HttpStatusCode.OK)
                return new ReturnType<T>(JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync()), response);
            else
                return new ReturnType<T>(response);
        }

        private async Task<HttpResponseMessage> MakeRequest(string method, string url, HttpContent content = null)
        {
            if (method == "GET")
            {
                return await _client.GetAsync(url);
            }
            else if (method == "POST")
            {
                return await _client.PostAsync(url, content);
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }

    public class ReturnType<T>
    {
        public T ReturnObject { get; set; }
        public HttpResponseMessage ResponseMessage { get; set; }

        public ReturnType(HttpResponseMessage message)
        {
            ResponseMessage = message;
        }
        public ReturnType(T returnObject, HttpResponseMessage message)
        {
            ReturnObject = returnObject;
            ResponseMessage = message;
        }
    }
}
