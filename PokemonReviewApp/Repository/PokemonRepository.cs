using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly ApplicationDBContext _db;
        public PokemonRepository(ApplicationDBContext Db)
        {
            _db = Db;
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _db.Pokemons.OrderBy(p => p.Id).ToList();
        }
    }
}
