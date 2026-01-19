using System.ComponentModel.DataAnnotations;
using StoreApp.Ingredients;

namespace StoreApp.Products
{
    public class Product
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; } = null!;

        // Price property with validation - must be non-negative
        [Range(0, 999999999999.99, ErrorMessage = "Price must be a positive value")]
        public decimal Price { get; set; }

        // Navigation property for many-to-many relationship with Ingredients
        public ICollection<Ingredient> Ingredients { get; set; } = [];
    }


}
