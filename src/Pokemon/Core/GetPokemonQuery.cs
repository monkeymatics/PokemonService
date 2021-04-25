namespace PokemonCore.Core
{
    public class GetPokemonQuery : IQuery<PokemonResponse>
    {
        public string PokemonName { get; set; }
        public bool TranslateDescription { get; set; }
    }
}
