using PassionProject.Models;

namespace PassionProject.Interfaces
{
    public interface IDessertService
    {
        // base CRUD
        Task<IEnumerable<DessertDto>> ListDesserts();
        Task<DessertDto?> FindDessert(int id);
        Task<ServiceResponse> UpdateDessert(DessertDto dessertDto);
        Task<ServiceResponse> AddDessert(DessertDto dessertDto);
        Task<ServiceResponse> DeleteDessert(int id);

        // related methods
        Task<IEnumerable<DessertDto>> ListDessertsForIngredient(int id);
        Task<ServiceResponse> LinkDessertToIngredient(int dessertId, int ingredientId);
        Task<ServiceResponse> UnlinkDessertFromIngredient(int dessertId, int ingredientId);

    }
}
