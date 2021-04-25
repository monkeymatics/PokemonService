using Newtonsoft.Json;

namespace PokemonCore.Core
{
    public class Pokemon
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public PokemonHabitat Habitat { get; private set; }
        public bool IsLegendary { get; private set; }

        public Pokemon(string name, string description, string habitat, bool isLegendary)
        {
            Name = name;
            Description = description;
            Habitat = new PokemonHabitat { Name = habitat };
            IsLegendary = isLegendary;
        }
    }

    public class PokemonHabitat
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
