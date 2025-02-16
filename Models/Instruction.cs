using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace PassionProject.Models
{
    public class Instruction
    {
        public int InstructionId { get; set; }
        public string ChangeIngredientOption { get; set; }
        public string QtyOfIngredient { get; set; }

        // a instruction  belongs to one dessert
        public required virtual Dessert Dessert { get; set; }
        public int DessertId { get; set; }

        // a instruction belongs to one ingredient
        public required virtual Ingredient Ingredient { get; set; }
        public int IngredientId { get; set; }
        
    }

    public class InstructionDto
    {
        public int InstructionId { get; set; }
        public string ChangeIngredientOption { get; set; }
        public string QtyOfIngredient { get; set; }
        public int DessertId { get; set; }
        public int IngredientId { get; set; }
        
        //flattened from Instruction -> Dessert
        public string DessertName { get; set; }

        //flattened from Instruction -> Ingredient
        public string IngredientName { get; set; }
    }

}
