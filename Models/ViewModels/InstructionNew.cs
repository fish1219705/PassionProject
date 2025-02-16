namespace PassionProject.Models.ViewModels
{
    public class InstructionNew
    {
        //the ViewModel I need to package the options for the new Instruction

        // For a list of desserts to choose from
        public IEnumerable<DessertDto> AllDesserts { get; set; }

        // For a list of ingredients to choose from
        public IEnumerable<IngredientDto> AllIngredients { get; set; }
    }
}
