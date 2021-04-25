using Microsoft.AspNetCore.Mvc;
using PokemonCore.Core;
using System;
using System.Threading.Tasks;

namespace PokemonService.Api.Controllers
{
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IQueryHandler<GetPokemonQuery, PokemonResponse> _requestPokemonHandler;

        public PokemonController(IQueryHandler<GetPokemonQuery, PokemonResponse> requestPokemonHandler)
        {
            _requestPokemonHandler = requestPokemonHandler ?? throw new ArgumentException(nameof(requestPokemonHandler));
        }

        [HttpGet("pokemon/{type}")]
        public async Task<IActionResult> GetBasicDetails([FromRoute] string type)
        {
            return await ProcessRequest(type, true);
        }

        [HttpGet("pokemon/translated/{type}")]
        public async Task<IActionResult> GetTranslatedDetails([FromRoute] string type)
        {
            return await ProcessRequest(type, true);
        }

        private async Task<IActionResult> ProcessRequest(string type, bool translate)
        {
            //https://localhost:44372/pokemon/translated/sdfsdflksdf
            try
            {
                var pokemon = await _requestPokemonHandler.HandleAsync(
                    new GetPokemonQuery
                    {
                        PokemonName = type,
                        TranslateDescription = translate
                    }
                    )
                    ;
                return Ok(pokemon);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (ApplicationException appEx)
            {
                return StatusCode(500, appEx.Message);
            }
        }
    }
}
