using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PassionProject.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        public required string ReviewNumber { get; set; }
        public required string ReviewContent { get; set; }
        public DateTime ReviewTime { get; set; }
        public string ReviewUser { get; set; }


        //[ForeignKey("Dessert")]


        //A review belongs to one dessert
        //A dessert has many reviews
        public required virtual Dessert Dessert { get; set; }
        public int DessertId { get; set; }
        

    }
    public class ReviewDto
    {
        public int ReviewId { get; set; }
        public string ReviewNumber { get; set; }
        public string ReviewContent { get; set; }
        public DateTime ReviewTime { get; set; }
        public string ReviewUser { get; set; }
        public int DessertId { get; set; }

        // flattened from Review -> Dessert
        public string? DessertName { get; set; }
    }
}
