using System.ComponentModel.DataAnnotations;
using StoreApp.Products;

namespace StoreApp.Ingredients
{
    public class Ingredient
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ingredient name is required")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Ingredient name must be between 1 and 200 characters")]
        public string Name { get; set; } = null!;

        // Indicates whether this ingredient is currently in stock
        public bool InStock { get; set; } = true;

        // Navigation property for many-to-many relationship with Products
        public ICollection<Product> Products { get; set; } = [];
    }


}
