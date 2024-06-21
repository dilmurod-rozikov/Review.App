using ReviewApp.Data;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;
        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Review? GetReview(int id)
        {
            return _context.Reviews.Find(id);
        }

        public ICollection<Review> GetReviewsOfAPokemon(int pokemonId)
        {
            return _context.Reviews.Where(x => x.Pokemon.Id == pokemonId).ToList();
        }

        public bool ReviewExists(int id)
        {
            return _context.Reviews.Any(x => x.Id == id);
        }

        public ICollection<Review> GetReviews()
        {
            return _context.Reviews.ToList();
        }

        public bool CreateReview(Review review)
        {
            _context.Add(review);
            return _context.SaveChanges() > 0;
        }

        public bool UpdateReview(Review review)
        {
            _context.Update(review);
            return _context.SaveChanges() > 0;
        }

        public bool DeleteReview(Review review)
        {
            _context.Remove(review);
            return _context.SaveChanges() > 0;
        }
    }
}
