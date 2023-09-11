using Microsoft.EntityFrameworkCore.Query.Internal;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly ApplicationDBContext _db; 
        public OwnerRepository(ApplicationDBContext Db)
        {
            _db = Db;
        }

        public bool CreateOwner(int pokemonId, int countryId, Owner owner)
        {
            var pokemon = _db.Pokemons.Where(p => p.Id == pokemonId).FirstOrDefault();
            var country = _db.Countries.Where(c => c.Id == countryId).FirstOrDefault();

            owner.Country = country;

            var pokemoneOwner = new PokemonOwner()
            {
                Owner = owner,
                Pokemon = pokemon
            };

            _db.Add(pokemoneOwner);
            _db.Add(owner);

            return Saved();
        
        }

        public Owner GetOwner(int ownerId)
        {
            return _db.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerOfAPokemon(int pokeId)
        {
            return _db.PokemonOwners.Where(p => p.Pokemon.Id == pokeId).Select(o => o.Owner).ToList();
        }

        public ICollection<Owner> GetOwners()
        {
            return _db.Owners.OrderBy(o => o.Id).ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
            return _db.PokemonOwners.Where(p => p.OwnerId == ownerId).Select(p => p.Pokemon).ToList(); 
        }

        public bool OwnerExists(int ownerId)
        {
            return _db.Owners.Any(o => o.Id == ownerId);
        }

        public bool Saved()
        {
            var saved = _db.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateOwner(Owner owner)
        {
            _db.Update(owner);
            return Saved();
        }
    }
}
