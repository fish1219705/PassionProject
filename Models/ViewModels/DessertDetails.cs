namespace PassionProject.Models.ViewModels
{
    public class DessertDetails
    {
        //A dessert page must have a dessert
        public required DessertDto Dessert { get; set; }

        //A dessert page can have many ingredients
        public IEnumerable<IngredientDto> DessertIngredients { get; set; }

        // All ingredients
        // ListIngredients()
        public IEnumerable<IngredientDto>? AllIngredients{ get; set; }

        //A dessert page can have many reviews
        public IEnumerable<ReviewDto> DessertReviews { get; set; }

        // All reviews
        // ListReviews()
        public IEnumerable<ReviewDto>? AllReviews{ get; set; }

        // All instructions for this dessert
        public IEnumerable<InstructionDto>? DessertInstructions { get; set; }
    }
}
