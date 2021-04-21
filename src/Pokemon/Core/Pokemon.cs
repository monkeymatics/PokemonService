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

    public class TranslationRequest
    {
        [JsonProperty("text")]
        public string TextToTranslate { get; set; }
    }

    public class PokemonHabitat
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
