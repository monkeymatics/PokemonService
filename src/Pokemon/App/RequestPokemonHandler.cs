using PokemonCore.Core;
using System;
using System.Threading.Tasks;

namespace PokemonCore.App
{
    public class RequestPokemonHandler : IQueryHandler<GetPokemonQuery, PokemonResponse>
    {
        private readonly IPokemonProvider _pokemonProvider;
        private readonly ITranslationProvider _translationProvider;
        public RequestPokemonHandler(IPokemonProvider pokemonProvider,
            ITranslationProvider translationProvider)
        {
            _pokemonProvider = pokemonProvider ?? throw new ArgumentNullException(nameof(pokemonProvider));
            _translationProvider = translationProvider ?? throw new ArgumentNullException(nameof(translationProvider));
        }

        public async Task<PokemonResponse> HandleAsync(GetPokemonQuery query)
        {
            var pokemon = await _pokemonProvider.GetPokemonByNameAsync(query.PokemonName);

            var response = new PokemonResponse
            {
                Habitat = pokemon.Habitat.Name,
                Name = pokemon.Name,
                IsLegendary = pokemon.IsLegendary
            };

            if (query.TranslateDescription)
            {
                if (pokemon.Habitat.Name == "cave" || pokemon.IsLegendary)
                {
                    response.Description = await _translationProvider.GetTranslation(pokemon.Description, TranslationLanguage.Yoda);
                }
                else
                {
                    response.Description = await _translationProvider.GetTranslation(pokemon.Description, TranslationLanguage.Shakespeare);
                }
            }
            else
                response.Description = pokemon.Description;

            return response;
        }
    }
}
