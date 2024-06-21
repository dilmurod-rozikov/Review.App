using ReviewApp.Data;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly ApplicationDbContext _context;
        public OwnerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Owner? GetOwner(int ownerId)
        {
            return _context.Owners.Find(ownerId);
        }

        public ICollection<Owner> GetOwnersOfAPokemon(int pokemonId)
        {
            return _context.PokemonOwners.Where(p => p.PokemonId == pokemonId).Select(o => o.Owner).ToList();
        }

        public ICollection<Pokemon> GetPokemonsByOwner(int ownerId)
        {
            return _context.PokemonOwners.Where(p => p.OwnerId == ownerId).Select(o => o.Pokemon).ToList();
        }

        public bool OwnerExists(int ownerId)
        {
            return _context.Owners.Any(x => x.Id == ownerId);
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.ToList();
        }

        public bool CreateOwner(Owner owner)
        {
            _context.Add(owner);
            return _context.SaveChanges() > 0;
        }

        public bool UpdateOwner(Owner owner)
        {
            _context.Update(owner);
            return _context.SaveChanges() > 0;
        }

        public bool DeleteOwner(Owner owner)
        {
            _context.Remove(owner);
            return _context.SaveChanges() > 0;
        }
    }
}
