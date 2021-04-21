using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonCore.Core
{

    public class TranslationResponse
    {
        [JsonProperty("success")]
        public TranslationSuccess Success { get; set; }
        [JsonProperty("contents")]
        public TranslationContents ResponseContent { get; set; }
    }

    public class TranslationSuccess
    {
        [JsonProperty("total")]
        public int SuccessCount { get; set; }
    }

    public class TranslationContents
    {
        [JsonProperty("translated")]
        public string TranslatedText { get; set; }
        [JsonProperty("text")]
        public string OriginalText { get; set; }
        [JsonProperty("translation")]
        public string Language { get; set; }
    }
}
