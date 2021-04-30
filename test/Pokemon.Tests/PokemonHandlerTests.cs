using Moq;
using Newtonsoft.Json;
using PokemonCore.App;
using PokemonCore.Core;
using PokemonCore.Infrastructure;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace PokemonService.Tests
{
    public class PokemonHandlerTests
    {
        private GetPokemonHandler _handler;
        private Mock<IHttpClient> _clientMock;
        private string shakespeareTranslation = "shakespeare";
        private string yodaTranslation = "yoda";
        private string originalTranslation = "original";
        private void Setup(bool isValid, bool isLegendary = false, string habitat = "notcave", bool translate = false)
        {
            _clientMock = new Mock<IHttpClient>();

            //CaveLegendary

            _clientMock.Setup(mock => mock.GetAsync<PokemonProviderResponse>(It.IsAny<string>()))
                .ReturnsAsync
                ((string input) =>
                new ReturnType<PokemonProviderResponse>
                (
                    new PokemonProviderResponse { Name = input.Replace($"{UrlConstants.PokemonApiBaseUrl}pokemon/", ""), PokemonId = 1 },
                    new HttpResponseMessage
                    {
                        StatusCode = isValid ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.NotFound,
                        Content = new StringContent(JsonConvert.SerializeObject(
                            new Pokemon(input.Replace($"{UrlConstants.PokemonApiBaseUrl}pokemon/", ""),
                                translate ? habitat == "cave" || isLegendary ? yodaTranslation : shakespeareTranslation : originalTranslation,
                                habitat,
                                isLegendary
                                )
                            )

                        )
                    }
                    ));

            _clientMock.Setup(mock => mock.GetAsync<PokemonSpeciesResponse>(It.IsAny<string>()))
                .ReturnsAsync
                ((string input) =>
                new ReturnType<PokemonSpeciesResponse>
                (
                    new PokemonSpeciesResponse
                    {
                        Habitat = new PokemonHabitatResponse
                        {
                            HabitatName = habitat
                        },
                        IsLegendary = isLegendary,
                        Descriptions = new List<PokemonDescription>
                        {
                            new PokemonDescription
                            {
                                Description = translate ? habitat == "cave" || isLegendary ? yodaTranslation : shakespeareTranslation : originalTranslation,
                                Language = new PokemonDescriptionAttribute{ Name = "en" },
                                Version = new PokemonDescriptionAttribute{ Name = "1" }
                            }
                        }
                    },
                    new HttpResponseMessage
                    {
                        StatusCode = isValid ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.NotFound,
                        Content = new StringContent(JsonConvert.SerializeObject(
                            new Pokemon(input.Replace($"{UrlConstants.PokemonApiBaseUrl}pokemon/", ""),
                                translate ? habitat == "cave" || isLegendary ? yodaTranslation : shakespeareTranslation : originalTranslation,
                                habitat,
                                isLegendary
                                )
                            )

                        )
                    }
                    ));

            _clientMock.Setup(mock => mock.GetAsync<TranslationResponse>(It.IsAny<string>()))
                .ReturnsAsync((string input) =>
                new ReturnType<TranslationResponse>
                (
                    new TranslationResponse
                    {
                        Success = new TranslationSuccess
                        {
                            SuccessCount = 1
                        },
                        ResponseContent = new TranslationContents
                        {
                            TranslatedText = input.StartsWith(UrlConstants.YodaTranslatorUrl) ? yodaTranslation : shakespeareTranslation
                        }
                    },
                new HttpResponseMessage
                {
                    Content = new StringContent
                    (
                        JsonConvert.SerializeObject
                        (
                            new TranslationResponse
                            {
                                ResponseContent = new TranslationContents { TranslatedText = yodaTranslation }
                            }
                            )
                        )
                }
                )
                );

            _clientMock.Setup(mock => mock.PostAsync(It.Is<string>(x => x.StartsWith(UrlConstants.ShakespeareTranslatorUrl)), It.IsAny<HttpContent>()))
                .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent
                    (
                        JsonConvert.SerializeObject
                        (
                            new TranslationResponse
                            {
                                ResponseContent = new TranslationContents { TranslatedText = shakespeareTranslation }
                            }
                            )
                        )
                }
                );

            _handler = new GetPokemonHandler(new PokemonProvider(_clientMock.Object), new TranslationProvider(_clientMock.Object));
        }

        [Fact]
        public async void Translated_Cave_Habitat_And_Legendary_Returns_Yoda()
        {
            Setup(true, true, "cave", true);
            var response = await _handler.HandleAsync(new GetPokemonQuery { PokemonName = "Test", TranslateDescription = true });

            Assert.Equal(yodaTranslation, response.Description);
        }

        [Fact]
        public async void Translated_Non_Cave_Habitat_And_Legendary_Returns_Yoda()
        {
            Setup(true, true, "noncave", true);
            var response = await _handler.HandleAsync(new GetPokemonQuery { PokemonName = "Test", TranslateDescription = true });

            Assert.Equal(yodaTranslation, response.Description);
        }

        [Fact]
        public async void Translated_Cave_Habitat_And_Non_Legendary_Returns_Yoda()
        {
            Setup(true, false, "cave", true);
            var response = await _handler.HandleAsync(new GetPokemonQuery { PokemonName = "Test", TranslateDescription = true });

            Assert.Equal(yodaTranslation, response.Description);
        }

        [Fact]
        public async void Translated_Non_Cave_Habitat_And_Non_Legendary_Returns_Shakespeare()
        {
            Setup(true, false, "noncave", true);
            var response = await _handler.HandleAsync(new GetPokemonQuery { PokemonName = "Test", TranslateDescription = true });

            Assert.Equal(shakespeareTranslation, response.Description);
        }

        [Fact]
        public async void Non_Translated_Cave_Habitat_And_Legendary_Returns_Yoda()
        {
            Setup(true, true, "cave", false);
            var response = await _handler.HandleAsync(new GetPokemonQuery { PokemonName = "Test", TranslateDescription = false });

            Assert.Equal(originalTranslation, response.Description);
        }

        [Fact]
        public async void Non_Translated_Non_Cave_Habitat_And_Legendary_Returns_Yoda()
        {
            Setup(true, true, "noncave", false);
            var response = await _handler.HandleAsync(new GetPokemonQuery { PokemonName = "Test", TranslateDescription = false });

            Assert.Equal(originalTranslation, response.Description);
        }

        [Fact]
        public async void Non_Translated_Cave_Habitat_And_Non_Legendary_Returns_Yoda()
        {
            Setup(true, false, "cave", false);
            var response = await _handler.HandleAsync(new GetPokemonQuery { PokemonName = "Test", TranslateDescription = false });

            Assert.Equal(originalTranslation, response.Description);
        }

        [Fact]
        public async void Non_Translated_Non_Cave_Habitat_And_Non_Legendary_Returns_Shakespeare()
        {
            Setup(true, false, "noncave", false);
            var response = await _handler.HandleAsync(new GetPokemonQuery { PokemonName = "Test", TranslateDescription = false });

            Assert.Equal(originalTranslation, response.Description);
        }
    }
}
