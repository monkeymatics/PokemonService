using Newtonsoft.Json;
using PokemonCore.Core;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PokemonCore.Infrastructure
{
    public class PokemonProvider : IPokemonProvider
    {
        private readonly IHttpClient _client;
        private readonly string _urlStub = "https://pokeapi.co/";

        public PokemonProvider(IHttpClient client)
        {
            _client = client;
        }

        public async Task<Pokemon> GetPokemonByNameAsync(string pokemonName)
        {
            var url = $"{UrlConstants.PokemonApiBaseUrl}pokemon/{pokemonName}";
            var response = await _client.GetAsync(url);
            
            return response.StatusCode == System.Net.HttpStatusCode.OK ? JsonConvert.DeserializeObject<Pokemon>(await response.Content.ReadAsStringAsync()) : throw new ArgumentException(nameof(pokemonName));
        }
    }
}
