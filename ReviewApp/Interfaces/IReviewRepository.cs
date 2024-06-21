using ReviewApp.Models;

namespace ReviewApp.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();

        Review? GetReview(int id);

        ICollection<Review> GetReviewsOfAPokemon(int pokemonId);

        bool ReviewExists(int id);

        bool CreateReview(Review review);
    }
}
