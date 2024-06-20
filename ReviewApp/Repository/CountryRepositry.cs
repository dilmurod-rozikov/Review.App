using ReviewApp.Data;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Repository
{
    public class CountryRepositry : ICountryRepositry
    {
        private readonly ApplicationDbContext _context;

        public CountryRepositry(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool CountryExists(int id)
        {
            return _context.Countries.Any(c => c.Id == id);
        }

        public Country? GetCountry(int id)
        {
            return _context.Countries.Find(id);
        }

        public Country? GetCountryByOwner(int id)
        {
            return _context.Owners.Where(o => o.Id == id).Select(c => c.Country).FirstOrDefault();
        }

        public ICollection<Country> GetCountryList()
        {
            return _context.Countries.ToList();
        }

        public ICollection<Owner> GetOwnerListFromACountry(int countryId)
        {
            return _context.Owners.Where(o => o.Country.Id == countryId).ToList();
        }
    }
}
