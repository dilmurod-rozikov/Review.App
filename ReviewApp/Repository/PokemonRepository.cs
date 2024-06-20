﻿using ReviewApp.Data;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly ApplicationDbContext _context;
        public PokemonRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Pokemon? GetPokemon(int id)
        {
            return _context.Pokemons.Find(id);
        }

        public Pokemon? GetPokemon(string name)
        {
            return _context.Pokemons.Where(p => p.Name == name).FirstOrDefault();
        }

        public decimal GetPokemonRating(int id)
        {
            var review = _context.Reviews.Where(p => p.Id == id);
            if (!review.Any())
                return 0.0m;

            return (decimal)review.Sum(p => p.Rating) / review.Count();
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemons.ToList();
        }

        public bool PokemonExists(int id)
        {
            return _context.Pokemons.Any(p => p.Id == id);
        }
    }
}
