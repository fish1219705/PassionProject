namespace PassionProject.Models.ViewModels
{
    public class InstructionDetails
    {

        // An instruction page must have an instruction
        // FindInstruction(instructionid)

        public required InstructionDto Instruction { get; set; }

        // An instruction may have a dessert associated to it
        public IEnumerable<DessertDto>? InstructionDessert { get; set; }

        // An instruction may have a ingredient associated to it
        public IEnumerable<IngredientDto>? InstructionIngredient { get; set; }

   

    
            


    }
}
