using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonCore.Core
{
    public class GetPokemonQuery : IQuery<Pokemon>
    {
        public string PokemonName { get; set; }
    }
}
