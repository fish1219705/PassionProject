namespace PassionProject.Models.ViewModels
{
    public class InstructionEdit
    {
        // Instruction to edit
        public required InstructionDto Instruction { get; set; }

        //choose which dessert the instruction refers
        public required IEnumerable<DessertDto> DessertOptions { get; set; }

        //choose which ingredient the order item refers
        public required IEnumerable<IngredientDto> IngredientOptions { get; set; }
    }
}
