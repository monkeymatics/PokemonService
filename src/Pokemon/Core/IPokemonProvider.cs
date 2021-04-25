using System.Threading.Tasks;

namespace PokemonCore.Core
{
    public interface IPokemonProvider
    {
        Task<Pokemon> GetPokemonByNameAsync(string pokemonName, string language = "en");
    }
}
