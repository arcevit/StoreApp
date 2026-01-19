using StoreApp.Exceptions;
using Microsoft.EntityFrameworkCore;
using StoreApp.Context;
using StoreApp.Products.Models;
using AutoMapper;

namespace StoreApp.Products
{
    public class ProductsService(StoreDbContext context, IMapper mapper) : IProductService
    {
        private readonly StoreDbContext _context = context;
        private readonly IMapper _mapper = mapper;


        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            // Include ingredients when fetching all products
            var products = await _context.Products
                .Include(p => p.Ingredients)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetById(int id)
        {

            // Include ingredients when fetching a single product
            var product = await _context.Products
                .Include(p => p.Ingredients)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            return product != null ? _mapper.Map<ProductDto>(product) : null;
        }

        public async Task<ProductDto> AddOne(ProductSaveDto productPayload)
        {

            var createdEntity = _mapper.Map<Product>(productPayload);

            _context.Products.Add(createdEntity);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductDto>(createdEntity);
        }

        public async Task UpdateOne(int id, ProductSaveDto productPayload)
        {
            var product = await _context.Products.FindAsync(id) ?? throw new NotFoundException(nameof(Product), id);
            var productEntity = _mapper.Map(productPayload, product);

            _context.Products.Update(productEntity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOne(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new NotFoundException(nameof(Product), id);
            }

            try
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting product with id {id}: {ex.Message}", ex);
            }
        }

        public async Task<ProductDto> AddIngredient(int productId, int ingredientId)
        {
            var product = await _context.Products
                .Include(p => p.Ingredients)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                throw new NotFoundException(nameof(Product), productId);
            }

            var ingredient = await _context.Ingredients.FindAsync(ingredientId) ?? throw new NotFoundException("Ingredient", ingredientId);

            // Check if ingredient is already assigned to this product
            if (product.Ingredients.Any(i => i.Id == ingredientId))
            {
                throw new ValidationException($"Ingredient with id {ingredientId} is already assigned to product {productId}");
            }

            product.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> RemoveIngredient(int productId, int ingredientId)
        {
            var product = await _context.Products
                .Include(p => p.Ingredients)
                .FirstOrDefaultAsync(p => p.Id == productId) ?? throw new NotFoundException(nameof(Product), productId);

            var ingredient = product.Ingredients.FirstOrDefault(i => i.Id == ingredientId) ?? throw new NotFoundException($"Ingredient with id {ingredientId} is not assigned to product {productId}");

            product.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductDto>(product);
        }
    }
}
