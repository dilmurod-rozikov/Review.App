using ReviewApp.Models;

namespace ReviewApp.DTO
{
    public class OwnerDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Gym { get; set; }

        public Country? Country { get; set; }
    }
}
