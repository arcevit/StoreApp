using StoreApp.Exceptions;
using Microsoft.EntityFrameworkCore;
using StoreApp.Context;
using StoreApp.Ingredients.Models;
using AutoMapper;

namespace StoreApp.Ingredients
{
    public class IngredientsService(StoreDbContext context, IMapper mapper) : IIngredientService
    {
        private readonly StoreDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<IngredientDto>> GetAll()
        {
            // Include products when fetching ingredients
            var ingredients = await _context.Ingredients
                .Include(i => i.Products)
                .AsNoTracking()
                .ToListAsync();


            return _mapper.Map<IEnumerable<IngredientDto>>(ingredients);
        }

        public async Task<IngredientDto?> GetById(int id)
        {
            // Include products when fetching a single ingredient
            var ingredient = await _context.Ingredients
                .Include(i => i.Products)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);

            return _mapper.Map<IngredientDto?>(ingredient);
        }

        public async Task<IngredientDto> AddOne(IngredientSaveDto ingredientPayload)
        {
            var createdEntity = _mapper.Map<Ingredient>(ingredientPayload);

            _context.Ingredients.Add(createdEntity);
            await _context.SaveChangesAsync();

            return _mapper.Map<IngredientDto>(createdEntity);
        }

        public async Task UpdateOne(int id, IngredientSaveDto ingredientPayload)
        {
            var ingredient = await _context.Ingredients.FindAsync(id) ?? throw new NotFoundException(nameof(Ingredient), id);
            var ingredientEntity = _mapper.Map(ingredientPayload, ingredient);

            _context.Ingredients.Update(ingredientEntity);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteOne(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id) ?? throw new NotFoundException(nameof(Ingredient), id);

            try
            {
                _context.Ingredients.Remove(ingredient);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting ingredient with id {id}: {ex.Message}", ex);
            }
        }

        public async Task<IngredientDto> UpdateInStock(int id, bool inStock)
        {
            var ingredient = await _context.Ingredients
                .Include(i => i.Products)
                .FirstOrDefaultAsync(i => i.Id == id) ?? throw new NotFoundException(nameof(Ingredient), id);

            ingredient.InStock = inStock;
            await _context.SaveChangesAsync();

            return _mapper.Map<IngredientDto>(ingredient);
        }

        public async Task UpdateAllInStock(bool inStock)
        {
            var ingredients = await _context.Ingredients
                .Include(i => i.Products)
                .ToListAsync();

            foreach (var ingredient in ingredients)
            {
                ingredient.InStock = inStock;
            }

            await _context.SaveChangesAsync();
        }
    }
}
