using System.Composition.Convention;

namespace PassionProject.Models
{
    public class DessertIngredient
    {
        public int DessertIngredientId { get; set; }

        public string ChangeIngredientOption { get; set; }

        // a dessert ingredient (info) belongs to one dessert
        public required virtual Dessert Dessert { get; set; }
        public int DessertId { get; set; }

        // a dessert ingredient (info) belongs to one ingredient
        public required virtual Ingredient Ingredient { get; set; }
        public int IngredientId { get; set; }
        
    }

    public class DessertIngredientDto
    {
        public int? DessertIngredientId { get; set; }
        public string ChangeIngredientOption { get; set; }
        public int DessertId { get; set; }
        public int IngredientId { get; set; }
        
        //flattened from DessertIngredient -> Dessert
        public string DessertName { get; set; }

        //flattened from DessertIngredient -> Ingredient
        public string IngredientName { get; set; }
    }

}
