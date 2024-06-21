using ReviewApp.Models;

namespace ReviewApp.Interfaces
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetPokemons();

        Pokemon? GetPokemon(int id);

        Pokemon? GetPokemon(string name);

        decimal GetPokemonRating(int id);

        bool PokemonExists(int id);

        bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);

        bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon);

        bool DeletePokemon(Pokemon pokemon);
    }
}
