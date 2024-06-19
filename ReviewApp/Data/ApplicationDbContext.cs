using Microsoft.EntityFrameworkCore;
using ReviewApp.Models;

namespace ReviewApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Pokemon> Pokemons { get; set; }

        public DbSet<Owner> Owners { get; set; }

        public DbSet<Country> Countrys { get; set; }

        public DbSet<PokemonCategory> PokemonCategories { get; set; }

        public DbSet<PokemonOwner> PokemonOwners { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Reviewer> Reviewers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PokemonCategory>()
                .HasKey(pc => new { pc.PokemonId, pc.CategoryId });

            modelBuilder.Entity<PokemonCategory>()
                .HasOne(p => p.Pokemon)
                .WithMany(pc => pc.PokemonCategories)
                .HasForeignKey(p => p.PokemonId);

            modelBuilder.Entity<PokemonCategory>()
               .HasOne(p => p.Category)
               .WithMany(pc => pc.PokemonCategories)
               .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<PokemonOwner>()
                .HasKey(pc => new { pc.PokemonId, pc.OwnerId });

            modelBuilder.Entity<PokemonOwner>()
                .HasOne(p => p.Pokemon)
                .WithMany(pc => pc.PokemonOwners)
                .HasForeignKey(p => p.PokemonId);

            modelBuilder.Entity<PokemonOwner>()
               .HasOne(p => p.Owner)
               .WithMany(pc => pc.PokemonOwners)
               .HasForeignKey(p => p.OwnerId);
        }
    }
}
