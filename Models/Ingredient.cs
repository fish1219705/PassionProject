using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace PassionProject.Models
{
    public class Ingredient
    {
        [Key]
        public int IngredientId { get; set; }
        public required string IngredientName { get; set; }
        public required string IngredientDescription { get; set; }

        //An ingredient can be used in many desserts
        public ICollection<Dessert>? Desserts { get; set; }

        // to do::
        // An ingredient can be part of many instructions
        // public ICollection<Instruction>? Ingredients { get; set; }

    }

    public class IngredientDto
    {
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public string IngredientDescription { get; set; }

    }
}
