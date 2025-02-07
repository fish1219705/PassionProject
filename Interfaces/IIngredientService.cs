using PassionProject.Models;

namespace PassionProject.Interfaces
{
    public interface IIngredientService
    {
       // base CRUD
       Task<IEnumerable<IngredientDto>> ListIngredients();
       Task<IngredientDto?> FindIngredient(int id);
       Task<ServiceResponse> UpdateIngredient(IngredientDto ingredientDto);
       Task<ServiceResponse> AddIngredient(IngredientDto ingredientDto);
        Task<ServiceResponse> DeleteIngredient(int id);

       // related methods

       Task<IEnumerable<IngredientDto>> ListIngredientsForDessert(int id);
 
    }
}

