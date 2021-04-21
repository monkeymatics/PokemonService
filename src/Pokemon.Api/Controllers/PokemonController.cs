using Microsoft.AspNetCore.Mvc;
using PokemonCore;
using PokemonCore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonService.Api.Controllers
{
    [Route("pokemon")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IQueryHandler<GetPokemonQuery, PokemonResponse> _requestPokemonHandler;

        public PokemonController(IQueryHandler<GetPokemonQuery, PokemonResponse> requestPokemonHandler)
        {
            _requestPokemonHandler = requestPokemonHandler ?? throw new ArgumentException(nameof(requestPokemonHandler));
        }

        [HttpGet("pokemon/{pokemonName}")]
        public async Task<IActionResult> GetBasicDetails([FromRoute] string pokemonName)
        {
            var pokemon = await _requestPokemonHandler.HandleAsync(
                new GetPokemonQuery 
                { 
                    PokemonName = pokemonName, 
                    TranslateDescription = false 
                }
                )
                ;
            return Ok(pokemon);
        }

        [HttpGet("pokemon/translated/{pokemonName}")]
        public async Task<IActionResult> GetTranslatedDetails([FromRoute] string pokemonName)
        {
            var pokemon = await _requestPokemonHandler.HandleAsync(
                new GetPokemonQuery
                {
                    PokemonName = pokemonName,
                    TranslateDescription = true
                }
                )
                ;
            return Ok(pokemon);
        }
    }
}
