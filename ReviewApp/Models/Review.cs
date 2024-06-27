using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReviewApp.Models
{
    [Table("Review")]
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        public int Rating { get; set; }

        public Reviewer? Reviewer { get; set; }

        public Pokemon? Pokemon { get; set; }
    }
}
