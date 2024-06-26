using System.ComponentModel.DataAnnotations.Schema;

namespace ReviewApp.Models
{
    [Table("PokemonOwner")]
    public class PokemonOwner
    {
        public int PokemonId { get; set; }

        public int OwnerId { get; set; }

        [ForeignKey("PokemonId")]
        public Pokemon Pokemon { get; set; }

        [ForeignKey("OwnerId")]
        public Owner Owner { get; set; }
    }
}
