using PokemonCore.Core;
using System;
using Xunit;

namespace PokemonService.Tests
{
    public class PokemonTests
    {

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Null_Or_Empty_Name_Throws_Exception(string name)
        {
            Assert.Throws<ArgumentNullException>(() => new Pokemon(name, "Test", "Test", true));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Null_Or_Empty_Description_Throws_Exception(string description)
        {
            Assert.Throws<ArgumentNullException>(() => new Pokemon("Test", description, "Test", true));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Null_Or_Empty_Habitat_Throws_Exception(string habitat)
        {
            Assert.Throws<ArgumentNullException>(() => new Pokemon("Test", "Test", habitat, true));
        }
    }
}
