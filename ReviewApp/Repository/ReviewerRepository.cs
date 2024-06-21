using ReviewApp.Data;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly ApplicationDbContext _context;
        public ReviewerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            _context.Add(reviewer);
            return _context.SaveChanges() > 0;
        }

        public ICollection<Reviewer> GetAll()
        {
            return _context.Reviewers.ToList();
        }

        public Reviewer? GetById(int id)
        {
            return _context.Reviewers.Find(id);
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerId)
        {
            return _context.Reviews.Where(x => x.Reviewer.Id == reviewerId).ToList();
        }

        public bool ReviewerExists(int reviewerId)
        {
            return _context.Reviewers.Any(x => x.Id == reviewerId);
        }
    }
}
