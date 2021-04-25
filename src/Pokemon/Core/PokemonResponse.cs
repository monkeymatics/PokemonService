using Newtonsoft.Json;

namespace PokemonCore.Core
{
    public class PokemonResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("habitat")]
        public string Habitat { get; set; }
        [JsonProperty("is_legendary")]
        public bool IsLegendary { get; set; }
    }
}
