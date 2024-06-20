using ReviewApp.Models;

namespace ReviewApp.Interfaces
{
    public interface ICountryRepositry
    {
        ICollection<Country> GetCountryList();

        Country? GetCountry(int id);

        Country? GetCountryByOwner(int id);

        ICollection<Owner> GetOwnerListFromACountry(int countryId);

        bool CountryExists(int id);
    }
}
