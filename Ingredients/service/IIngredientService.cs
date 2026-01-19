using StoreApp.Ingredients.Models;

namespace StoreApp.Ingredients
{
    public interface IIngredientService
    {
        Task<IEnumerable<IngredientDto>> GetAll();
        Task<IngredientDto?> GetById(int id);
        Task<IngredientDto> AddOne(IngredientSaveDto dto);
        Task UpdateOne(int id, IngredientSaveDto dto);
        Task DeleteOne(int id);
        Task<IngredientDto> UpdateInStock(int id, bool inStock);
        Task UpdateAllInStock(bool inStock);
    }
}
