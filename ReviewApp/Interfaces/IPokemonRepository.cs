using ReviewApp.Models;

namespace ReviewApp.Interfaces
{
    public interface IPokemonRepository
    {
        public ICollection<Pokemon> GetPokemons();

        Pokemon? GetPokemon(int id);

        Pokemon? GetPokemon(string name);

        decimal GetPokemonRating(int id);

        bool PokemonExists(int id);
    }
}
