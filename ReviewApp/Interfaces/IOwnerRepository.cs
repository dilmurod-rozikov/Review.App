using ReviewApp.Models;

namespace ReviewApp.Interfaces
{
    public interface IOwnerRepository
    {
        ICollection<Owner> GetOwners();

        Owner? GetOwner(int ownerId);

        ICollection<Owner> GetOwnersOfAPokemon(int pokemonId);

        ICollection<Pokemon> GetPokemonsByOwner(int ownerId);

        bool OwnerExists(int ownerId);
    }
}
