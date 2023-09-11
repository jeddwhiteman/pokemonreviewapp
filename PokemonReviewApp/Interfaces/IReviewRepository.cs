using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int reviewId);
        ICollection<Review> GetReviewsOfAPokemon( int pokeId);
        bool ReviewExists(int reviewId);

        bool CreateReview (int reviewerId, int pokemonId, Review review);

        bool Saved();
    }
}
