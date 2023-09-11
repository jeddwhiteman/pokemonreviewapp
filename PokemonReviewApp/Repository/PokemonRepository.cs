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

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var pokemonOwnerEntity = _db.Owners.Where(a => a.Id == ownerId).FirstOrDefault();
            var category = _db.Categories.Where(c => c.Id == categoryId).FirstOrDefault();

            var pokemonOwner = new PokemonOwner()
            {
                Owner = pokemonOwnerEntity,
                Pokemon = pokemon,
            };

            _db.Add(pokemonOwner);

            var pokemonCategory = new PokemonCategory()
            {
                Category = category,
                Pokemon = pokemon,
            };

            _db.Add(pokemonCategory);
            _db.Add(pokemon);

            return Save();
        }

        public Pokemon GetPokemon(int id)
        {
            return _db.Pokemons.Where(p => p.Id == id).FirstOrDefault();
        }

        public Pokemon GetPokemon(string name)
        {
            return _db.Pokemons.Where(p => p.Name == name).FirstOrDefault();
        }

        public decimal GetPokemonAverageRating(int pokeId)
        {
            var review = _db.Reviews.Where(p => p.Pokemon.Id == pokeId);
            if (review.Count() <= 0)
            {
                return 0;
            }

            return ((decimal)review.Sum(r => r.Rating) / review.Count());
        }

        public decimal GetPokemonAverageRatingByReviewerID(int reviewerID, int pokeId)
        {
            var review = _db
                .Reviews
                .Where(p => p.Reviewer.Id == reviewerID && p.Pokemon.Id == pokeId)
                .Select(p => p.Rating)
                .FirstOrDefault();

            return review;
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _db.Pokemons.OrderBy(p => p.Id).ToList();
        }

        public bool PokemonExists(int id)
        {
            return _db.Pokemons.Any(p => p.Id == id);
        }

        public bool Save()
        {
            var saved = _db.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
