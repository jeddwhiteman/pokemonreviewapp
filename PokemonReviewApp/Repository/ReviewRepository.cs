using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDBContext _db;

        public ReviewRepository(ApplicationDBContext db)
        {
            _db = db;   
        }
        
        public ICollection<Review> GetReviewsOfAPokemon(int pokeId)
        {
            return _db.Reviews.Where(r => r.Pokemon.Id == pokeId).ToList();
        }

        public Review GetReview(int reviewId)
        {
            return _db.Reviews.Where(r => r.Id == reviewId).FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return _db.Reviews.OrderBy(r => r.Id).ToList();
        }

        public bool ReviewExists(int reviewId)
        {
            return _db.Reviews.Any(r => r.Id == reviewId);
        }
    }
}
