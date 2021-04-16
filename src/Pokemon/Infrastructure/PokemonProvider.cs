using Newtonsoft.Json;
using PokemonCore.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonCore.Infrastructure
{
    public class PokemonProvider : IPokemonProvider
    {
        private readonly IHttpClient _client;

        public PokemonProvider(IHttpClient client)
        {
            _client = client;
        }

        public async Task<Pokemon> GetPokemonByNameAsync(string pokemonName, string language = "en")
        {
            var url = $"{UrlConstants.PokemonApiBaseUrl}pokemon/{pokemonName}";
            var response = await _client.GetAsync<PokemonResponse>(url);

            if (response.ResponseMessage.StatusCode != System.Net.HttpStatusCode.OK)
                throw new ArgumentException(nameof(pokemonName));

            var pokemonResponse = response.ReturnObject;

            url = $"{UrlConstants.PokemonApiBaseUrl}pokemon-species/{pokemonName}";
            var speciesResponse = await _client.GetAsync<PokemonSpeciesResponse>(url);

            var pokemonSpeciesResponse = speciesResponse.ReturnObject;

            var description = pokemonSpeciesResponse.Descriptions.FirstOrDefault(z => z.Language.Name == language);

            return new Pokemon(pokemonResponse.Name, description.Description, pokemonSpeciesResponse.Habitat.HabitatName, pokemonSpeciesResponse.IsLegendary);
        }
    }

    public class PokemonResponse
    {
        [JsonProperty("id")]
        public int PokemonId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class PokemonSpeciesResponse
    {
        [JsonProperty("is_legendary")]
        public bool IsLegendary { get; set; }
        [JsonProperty("habitat")]
        public PokemonHabitatResponse Habitat { get; set; }
        [JsonProperty("flavor_text_entries")]
        public List<PokemonDescription> Descriptions { get; set; }
    }

    public class PokemonHabitatResponse
    {
        [JsonProperty("name")]
        public string HabitatName { get; set; }
    }

    public class PokemonDescription
    {
        [JsonProperty("flavor_text")]
        public string Description { get; set; }
        [JsonProperty("language")]
        public PokemonDescriptionAttribute Language { get; set; }
        [JsonProperty("version")]
        public PokemonDescriptionAttribute Version { get; set; }
    }

    public class PokemonDescriptionAttribute
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
