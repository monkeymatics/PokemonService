using System;
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
            throw new NotImplementedException();
        }

        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent requestContent)
        {
            throw new NotImplementedException();
        }
    }
}
