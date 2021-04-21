using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PokemonCore.Core
{
    public interface ITranslationProvider
    {
        Task<string> GetTranslation(string text, TranslationLanguage language);
    }
}
