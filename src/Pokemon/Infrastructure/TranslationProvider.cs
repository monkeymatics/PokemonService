using PokemonCore.Core;
using System;
using System.Net.Http;
using System.Threading.Tasks;

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
            try
            {
                string url = language == TranslationLanguage.Yoda ? UrlConstants.YodaTranslatorUrl : UrlConstants.ShakespeareTranslatorUrl;

                url += $"?text={text}";

                Console.WriteLine($"Translation url - {url}");
                Console.WriteLine($"Httpclient null - {_httpClient == null}");

                var translation = await _httpClient.GetAsync<TranslationResponse>(url);

                if (translation.ResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    return translation.ReturnObject.Success.SuccessCount > 0 ? translation.ReturnObject.ResponseContent.TranslatedText : "Could not translate description";
                else
                    throw new ApplicationException(translation.ResponseMessage.ReasonPhrase);
            }
            catch (HttpRequestException)
            {
                throw new ApplicationException("An error occurred during translation");
            }
        }
    }
}
