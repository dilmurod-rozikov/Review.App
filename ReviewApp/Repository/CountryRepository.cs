using ReviewApp.Data;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly ApplicationDbContext _context;

        public CountryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool CountryExists(int id)
        {
            return _context.Countries.Any(c => c.Id == id);
        }

        public bool CreateCountry(Country country)
        {
            _context.Add(country);
            return _context.SaveChanges() > 0;
        }

        public Country? GetCountry(int id)
        {
            return _context.Countries.Find(id);
        }

        public Country? GetCountryByOwner(int id)
        {
            return _context.Owners.Where(o => o.Id == id).Select(c => c.Country).FirstOrDefault();
        }

        public ICollection<Country> GetCountries()
        {
            return _context.Countries.ToList();
        }

        public ICollection<Owner> GetOwnerListFromACountry(int countryId)
        {
            return _context.Owners.Where(o => o.Country.Id == countryId).ToList();
        }

        public bool UpdateCountry(Country country)
        {
            _context.Update(country);
            return _context.SaveChanges() > 0;
        }

        public bool DeleteCountry(Country country)
        {
            _context.Remove(country);
            return _context.SaveChanges() > 0;
        }
    }
}
