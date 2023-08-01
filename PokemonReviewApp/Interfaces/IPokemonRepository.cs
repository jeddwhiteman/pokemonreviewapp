using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetPokemons();
        Pokemon GetPokemon (int id);
        Pokemon GetPokemon (string name);
        decimal GetPokemonAverageRating(int pokeId);

        decimal GetPokemonAverageRatingByReviewerID(int reviewerID, int pokeId);
        bool PokemonExists (int id);
    }
}
