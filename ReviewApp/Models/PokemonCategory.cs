using System.ComponentModel.DataAnnotations.Schema;

namespace ReviewApp.Models
{
    [Table("PokemonCategory")]
    public class PokemonCategory
    {
        public int PokemonId { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey("PokemonId")]
        public Pokemon Pokemon { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}
