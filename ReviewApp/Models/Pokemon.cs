using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReviewApp.Models
{
    [Table("Pokemon")]
    public class Pokemon
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public DateOnly BirthDate { get; set; }

        public ICollection<Review> Reviews { get; set; }

        public ICollection<PokemonOwner> PokemonOwners { get; set; }

        public ICollection<PokemonCategory> PokemonCategories { get; set; }
    }
}
