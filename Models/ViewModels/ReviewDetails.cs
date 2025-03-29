namespace PassionProject.Models.ViewModels
{
    public class ReviewDetails
    {
        // A review page must have a review
        // FindReview(reviewid)
        public required ReviewDto Review { get; set; }


        // A review may have a dessert associated to it
        public IEnumerable<DessertDto>? ReviewDessert { get; set; }


        // All desserts
        // ListDesserts()
        public IEnumerable<DessertDto>? AllDesserts { get; set; }

    }
}
