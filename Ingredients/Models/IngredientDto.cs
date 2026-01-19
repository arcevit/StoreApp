using System.ComponentModel.DataAnnotations;
using StoreApp.Products.Models;

namespace StoreApp.Ingredients.Models
{
    public record IngredientDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
        public bool InStock { get; init; }
        public List<PartialProductDto> Products { get; init; } = [];
    }
    public record IngredientSaveDto
    {
        [Required(ErrorMessage = "Ingredient name is required")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Ingredient name must be between 1 and 200 characters")]
        public string Name { get; set; } = null!;

        public bool InStock { get; set; } = true;
    }
    public record PartialIngredientDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
        public bool InStock { get; init; }
    }
    // DTO for bulk updating InStock status
    public record BulkInStockUpdateDto
    {
        [Required(ErrorMessage = "InStock value is required")]
        public bool InStock { get; set; }
    }
}
