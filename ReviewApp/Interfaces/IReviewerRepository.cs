﻿using ReviewApp.Models;

namespace ReviewApp.Interfaces
{
    public interface IReviewerRepository
    {
        ICollection<Reviewer> GetAll();

        Reviewer? GetById(int id);

        ICollection<Review> GetReviewsByReviewer(int reviewerId);

        bool ReviewerExists(int reviewerId);

        bool CreateReviewer(Reviewer reviewer);

        bool UpdateReviewer(Reviewer reviewer);

        bool DeleteReviewer(Reviewer reviewer);
    }
}
