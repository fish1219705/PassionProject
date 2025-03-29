namespace PassionProject.Models.ViewModels
{
    public class IngredientDetails
    {
        // An ingredient page must have an ingredient
        // FindIngredient(ingredientid)

        public required IngredientDto Ingredient { get ; set; }

        // An ingredient may have desserts associated to it
        // ListDessertsForIngredient(ingredientid)
        public IEnumerable<DessertDto>? IngredientDesserts { get; set; }

        // All desserts
        // ListDesserts()
        public IEnumerable<DessertDto>? AllDesserts { get; set; }

        // All instructions for this ingredient
        public IEnumerable<InstructionDto>? IngredientInstructions { get; set; }
    }
}
