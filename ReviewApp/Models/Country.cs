using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReviewApp.Models
{
    [Table("Country")]
    public class Country
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }

        public ICollection<Owner> Owners { get; set; }
    }
}
