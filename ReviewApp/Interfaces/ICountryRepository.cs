using ReviewApp.Models;

namespace ReviewApp.Interfaces
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();

        Country? GetCountry(int id);

        Country? GetCountryByOwner(int id);

        ICollection<Owner> GetOwnerListFromACountry(int countryId);

        bool CountryExists(int id);

        bool CreateCountry(Country country);

        bool UpdateCountry(Country country);

        bool DeleteCountry(Country country);
    }
}
