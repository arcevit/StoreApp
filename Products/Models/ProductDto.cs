using System.ComponentModel.DataAnnotations;
using StoreApp.Ingredients.Models;

namespace StoreApp.Products.Models
{
    public class ProductDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
        public decimal Price { get; init; }
        public bool Available { get; init; }
        public List<PartialIngredientDto> Ingredients { get; init; } = [];
    }

    public class ProductSaveDto
    {
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Product name must be between 1 and 200 characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Price is required")]
        [Range(0, 999999999999.99, ErrorMessage = "Price must be a positive value")]
        public decimal Price { get; set; }
    }
    public record PartialProductDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
    }
}
