namespace PokemonCore.Core
{
    public class GetPokemonQuery : IQuery<GetPokemonQueryResult>
    {
        public string PokemonName { get; set; }
        public bool TranslateDescription { get; set; }
    }
}
