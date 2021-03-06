using System.Net.Http;
using System.Threading.Tasks;

namespace PokemonCore.Infrastructure
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string url);
        Task<ReturnType<T>> GetAsync<T>(string url);
        Task<HttpResponseMessage> PostAsync(string url, HttpContent requestContent);
        Task<ReturnType<T>> PostAsync<T>(string url, HttpContent requestContent);
    }
}
