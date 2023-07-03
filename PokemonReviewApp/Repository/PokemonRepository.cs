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

        public Pokemon GetPokemon(int id)
        {
            return _db.Pokemons.Where(p => p.Id == id).FirstOrDefault();
        }

        public Pokemon GetPokemon(string name)
        {
            return _db.Pokemons.Where(p => p.Name == name).FirstOrDefault();
        }

        public decimal GetPokemonRating(int pokeId)
        {
            var review = _db.Reviews.Where(p => p.Pokemon.Id == pokeId);
            if (review.Count() <= 0)
            {
                return 0;
            }

            return ((decimal)review.Sum(r => r.Rating) / review.Count());
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _db.Pokemons.OrderBy(p => p.Id).ToList();
        }

        public bool PokemonExists(int id)
        {
            return _db.Pokemons.Any(p => p.Id == id);
        }
    }
}
