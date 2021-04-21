using PokemonCore.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PokemonCore.Infrastructure
{
    public class TranslationProvider : ITranslationProvider
    {
        private readonly IHttpClient _httpClient;
        public TranslationProvider(IHttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<string> GetTranslation(string text, TranslationLanguage language)
        {
            string url = language == TranslationLanguage.Yoda ? UrlConstants.YodaTranslatorUrl : UrlConstants.ShakespeareTranslatorUrl;

            url += $"?text={HttpUtility.UrlEncode(text)}";

            var translation = await _httpClient.GetAsync<TranslationResponse>(url);

            return translation.ReturnObject.Success.SuccessCount > 0 ? translation.ReturnObject.ResponseContent.TranslatedText : "Could not translate description";
        }
    }
}
