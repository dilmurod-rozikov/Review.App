using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReviewApp.Models
{
    [Table("Owner")]
    public class Owner
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Gym { get; set; }

        public Country Country { get; set; }

        public ICollection<PokemonOwner> PokemonOwners { get; set; }
    }
}
