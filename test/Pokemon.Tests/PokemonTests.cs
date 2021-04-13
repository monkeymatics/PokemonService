using Moq;
using Newtonsoft.Json;
using PokemonCore.Core;
using PokemonCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;

namespace PokemonService.Tests
{
    public class PokemonTests
    {
        private Mock<IHttpClient> clientMock;
        private string shakespeareTranslation = "shakespeare";
        private string yodaTranslation = "yoda";
        private void Setup()
        {
            clientMock = new Mock<IHttpClient>();

            clientMock.Setup(mock => mock.PostAsync(It.Is<string>(x => x == UrlConstants.YodaTranslatorUrl), It.IsAny<HttpContent>()))
                .ReturnsAsync(
                new HttpResponseMessage 
                { 
                    Content = new StringContent
                    (
                        JsonConvert.SerializeObject
                        (
                            new TranslationResponse 
                            { 
                                ResponseContent = new TranslationResponseContents { TranslatedText = yodaTranslation } 
                            }
                            )
                        )
                }
                );

            clientMock.Setup(mock => mock.PostAsync(It.Is<string>(x => x == UrlConstants.ShakespeareTranslatorUrl), It.IsAny<HttpContent>()))
                .ReturnsAsync(
                new HttpResponseMessage
                {
                    Content = new StringContent
                    (
                        JsonConvert.SerializeObject
                        (
                            new TranslationResponse
                            {
                                ResponseContent = new TranslationResponseContents { TranslatedText = shakespeareTranslation }
                            }
                            )
                        )
                }
                );
        }

        [Fact]
        public async void Cave_Habitat_And_Legendary_Returns_Yoda()
        {
            Setup();
            var pokemon = new Pokemon()
            {
                Description = "Test",
                Habitat = new PokemonHabitat() { Name = "cave" },
                IsLegendary = true,
                Name = "Test"
            };

            var desc = await pokemon.GetDescription(clientMock.Object);

            Assert.Equal(yodaTranslation, desc);
        }

        [Fact]
        public async void Non_Cave_Habitat_And_Legendary_Returns_Yoda()
        {
            Setup();
            var pokemon = new Pokemon()
            {
                Description = "Test",
                Habitat = new PokemonHabitat() { Name = "forest" },
                IsLegendary = true,
                Name = "Test"
            };

            var desc = await pokemon.GetDescription(clientMock.Object);

            Assert.Equal(yodaTranslation, desc);
        }

        [Fact]
        public async void Cave_Habitat_And_Non_Legendary_Returns_Yoda()
        {
            Setup();
            var pokemon = new Pokemon()
            {
                Description = "Test",
                Habitat = new PokemonHabitat() { Name = "cave" },
                IsLegendary = false,
                Name = "Test"
            };

            var desc = await pokemon.GetDescription(clientMock.Object);

            Assert.Equal(yodaTranslation, desc);
        }

        [Fact]
        public async void Non_Cave_Habitat_And_Non_Legendary_Returns_Shakespeare()
        {
            Setup();
            var pokemon = new Pokemon()
            {
                Description = "Test",
                Habitat = new PokemonHabitat() { Name = "forest" },
                IsLegendary = false,
                Name = "Test"
            };

            var desc = await pokemon.GetDescription(clientMock.Object);

            Assert.Equal(shakespeareTranslation, desc);
        }


    }
}
