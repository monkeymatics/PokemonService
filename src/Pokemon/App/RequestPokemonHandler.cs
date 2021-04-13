using PokemonCore.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PokemonCore.App
{
    public class RequestPokemonHandler : IQueryHandler<GetPokemonQuery, Pokemon>
    {
        public Task<Pokemon> HandleAsync(GetPokemonQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
