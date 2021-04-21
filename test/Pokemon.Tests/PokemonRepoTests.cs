using Moq;
using Newtonsoft.Json;
using PokemonCore.Core;
using PokemonCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;
using Xunit.Sdk;

namespace PokemonService.Tests
{
    public class PokemonRepoTests
    {
        private IPokemonProvider provider;
        private Mock<IHttpClient> clientMock;
        private string shakespeareTranslation = "shakespeare";
        private string yodaTranslation = "yoda";

        private void Setup(bool isValid, bool isLegendary = false, string habitat = "notcave")
        {
            clientMock = new Mock<IHttpClient>();

            clientMock.Setup(mock => mock.GetAsync<TranslationResponse>(It.IsAny<string>()))
            .ReturnsAsync
            ((string input) =>
            new ReturnType<TranslationResponse>
            (
                new TranslationResponse
                {
                    ResponseContent = new TranslationContents
                    {
                        TranslatedText = input.StartsWith(UrlConstants.YodaTranslatorUrl) ? yodaTranslation : shakespeareTranslation
                    }
                },
                new HttpResponseMessage
                {
                    StatusCode = isValid ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.NotFound,
                    Content = new StringContent(JsonConvert.SerializeObject(
                            new TranslationResponse
                            {
                                ResponseContent = new TranslationContents
                                {
                                    TranslatedText = input.StartsWith(UrlConstants.YodaTranslatorUrl) ? yodaTranslation : shakespeareTranslation
                                }
                            }
                            )

                        )
                }

            )
                    );

            clientMock.Setup(mock => mock.GetAsync<PokemonProviderResponse>(It.IsAny<string>()))
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
                                habitat == "cave" || isLegendary ? "yoda" : "shakespeare",
                                habitat,
                                isLegendary
                                )
                            )

                        )
                    }
                    ));

            clientMock.Setup(mock => mock.GetAsync<PokemonSpeciesResponse>(It.IsAny<string>()))
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
                                Description = habitat == "cave" || isLegendary ? "yoda" : "shakespeare",
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
                                habitat == "cave" || isLegendary ? "yoda" : "shakespeare",
                                habitat,
                                isLegendary
                                )
                            )

                        )
                    }
                    ));


            provider = new PokemonProvider(clientMock.Object);
        }

        [Theory]
        [InlineData("Pikachu")]
        [InlineData("MewTwo")]
        public async void Can_Find_Valid_Pokemon(string pokemonName)
        {
            Setup(true);
            var pokemon = await provider.GetPokemonByNameAsync(pokemonName);

            Assert.Equal(pokemonName, pokemon.Name);
        }

        [Fact]
        public async void Legendary_Not_Cave_Habitat_Pokemon_Returns_Yoda_Description()
        {
            Setup(true, true, "notcave");

            var pokemon = await provider.GetPokemonByNameAsync("Pikachu");

            Assert.Equal("yoda", pokemon.Description);
        }

        [Fact]
        public async void Cave_Habitat_Not_Legendary_Returns_Yoda_Description()
        {
            Setup(true, false, "cave");

            var pokemon = await provider.GetPokemonByNameAsync("Pikachu");

            Assert.Equal("yoda", pokemon.Description);
        }

        [Fact]
        public async void Legendary_Cave_Habitat_Pokemon_Returns_Yoda_Description()
        {
            Setup(true, true, "cave");

            var pokemon = await provider.GetPokemonByNameAsync("Pikachu");

            Assert.Equal("yoda", pokemon.Description);
        }

        [Fact]
        public async void Cave_Habitat_Legendary_Returns_Yoda_Description()
        {
            Setup(true, true, "cave");

            var pokemon = await provider.GetPokemonByNameAsync("Pikachu");

            Assert.Equal("yoda", pokemon.Description);
        }

        [Theory]
        [InlineData("Fake")]
        [InlineData("Imaginary")]
        public async void Cannot_Find_Invalid_Pokemon(string pokemonName)
        {
            Setup(false);

            await Assert.ThrowsAsync<ArgumentException>(() => provider.GetPokemonByNameAsync(pokemonName));
        }
    }
}