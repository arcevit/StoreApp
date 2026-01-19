using StoreApp.Products.Models;

namespace StoreApp.Products
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAll();
        Task<ProductDto?> GetById(int id);
        Task<ProductDto> AddOne(ProductSaveDto dto);
        Task UpdateOne(int id, ProductSaveDto dto);
        Task DeleteOne(int id);
        Task<ProductDto> AddIngredient(int productId, int ingredientId);
        Task<ProductDto> RemoveIngredient(int productId, int ingredientId);
    }
}
