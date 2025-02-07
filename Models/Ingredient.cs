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

        ////An ingredient can be a part of many dessert ingredients
        //public ICollection<Instruction>? Ingredients { get; set; }


        //An ingredient can use in many desserts
        public ICollection<Dessert>? Desserts { get; set; }

    }

    public class IngredientDto
    {
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public string IngredientDescription { get; set; }

    }
}
