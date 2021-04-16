using Newtonsoft.Json;
using PokemonCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PokemonCore.Core
{
    public class Pokemon
    {
        [JsonProperty("name")]
        public string Name { get; private set; }
        [JsonProperty("description")]
        public string Description { get; private set; }
        [JsonProperty("habitat")]
        public PokemonHabitat Habitat { get; private set; }
        [JsonProperty("is_legendary")]
        public bool IsLegendary { get; private set; }

        public Pokemon(string name, string description, string habitat, bool isLegendary)
        {
            Name = name;
            Description = description;
            Habitat = new PokemonHabitat { Name = habitat };
            IsLegendary = isLegendary;
        }

        public async Task<string> GetDescription(IHttpClient httpClient)
        {
            var url = (Habitat.Name == "cave" || IsLegendary) ? UrlConstants.YodaTranslatorUrl : UrlConstants.ShakespeareTranslatorUrl;
            var content = new StringContent(JsonConvert.SerializeObject(new TranslationRequest { TextToTranslate = Description }));
            var translated = await (await httpClient.PostAsync(url, content)).Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TranslationResponse>(translated).ResponseContent.TranslatedText;
        }
    }

    public class TranslationRequest
    {
        [JsonProperty("text")]
        public string TextToTranslate { get; set; }
    }

    public class TranslationResponse
    {
        [JsonProperty("content")]
        public TranslationResponseContents ResponseContent { get; set; }
    }

    public class TranslationResponseContents
    {
        [JsonProperty("text")]
        public string OriginalText { get; set; }
        [JsonProperty("translated")]
        public string TranslatedText { get; set; }
    }

    public class PokemonHabitat
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
