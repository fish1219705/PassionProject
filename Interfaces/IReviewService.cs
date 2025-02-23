using PassionProject.Models;

namespace PassionProject.Interfaces
{
    public interface IReviewService
    {
        // base CRUD
        Task<IEnumerable<ReviewDto>> ListReviews();
        Task<ReviewDto?> FindReview(int id);
        Task<ServiceResponse> UpdateReview(ReviewDto reviewDto);
        Task<ServiceResponse> AddReview(ReviewDto reviewDto);
        Task<ServiceResponse> DeleteReview(int id);
        Task<ServiceResponse> UpdateReviewImage(int id, IFormFile ReviewPic);

        // related methods
        Task<IEnumerable<ReviewDto>> ListReviewsForDessert(int id);
    }
}
