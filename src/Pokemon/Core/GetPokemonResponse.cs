﻿using Newtonsoft.Json;

namespace PokemonCore.Core
{
    public class GetPokemonQueryResult
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Habitat { get; set; }
        public bool IsLegendary { get; set; }
    }
}
