using System;

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
            Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentNullException(nameof(name)) : name;
            Description = string.IsNullOrWhiteSpace(description) ? throw new ArgumentNullException(nameof(description)) : description;
            Habitat = new PokemonHabitat { Name = string.IsNullOrWhiteSpace(habitat) ? throw new ArgumentNullException(nameof(habitat)) : habitat };
            IsLegendary = isLegendary;
        }
    }

    public class PokemonHabitat
    {
        public string Name { get; set; }
    }
}
