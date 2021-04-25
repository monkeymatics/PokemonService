using System.Threading.Tasks;

namespace PokemonCore.Core
{
    public interface ITranslationProvider
    {
        Task<string> GetTranslation(string text, TranslationLanguage language);
    }
}
