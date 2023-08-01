using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly ApplicationDBContext _db;

        public CountryRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public bool CountryExists(int id)
        {
            return _db.Countries.Any(c => c.Id == id);
        }

        public ICollection<Country> GetCountries()
        {
            return _db.Countries.OrderBy(c => c.Id).ToList();
        }

        public Country GetCountry(int countryId)
        {
            return _db.Countries.Where(c => c.Id == countryId).FirstOrDefault();
        }


        public Country GetCountryByOwner(int ownerId)
        {
            return _db.Owners.Where(o => o.Id == ownerId).Select(c => c.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersFromCountry(int countryId)
        {
            return _db.Owners.Where(c => c.Country.Id == countryId).ToList();
        }
    }
}
