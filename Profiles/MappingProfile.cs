using AutoMapper;
using StoreApp.Ingredients;
using StoreApp.Ingredients.Models;
using StoreApp.Products;
using StoreApp.Products.Models;

// Mapping profile for AutoMapper to map between Ingredient and Product models and their DTOs

namespace StoreApp.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductDto, Product>();
            CreateMap<ProductSaveDto, Product>();

            // Product to ProductDto with custom Available logic
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Available,
                    opt => opt.MapFrom(src => src.Ingredients.Any() && src.Ingredients.All(i => i.InStock)));

            // Ingredient to PartialIngredientDto
            CreateMap<Ingredient, PartialIngredientDto>();
            CreateMap<IngredientDto, Ingredient>();
            CreateMap<IngredientSaveDto, Ingredient>();

            // Ingredient to IngredientDto with Products mapped to PartialProductDto
            CreateMap<Ingredient, IngredientDto>()
                .ForMember(dest => dest.Products,
                    opt => opt.MapFrom(src => src.Products.Select(p => new PartialProductDto
                    {
                        Id = p.Id,
                        Name = p.Name
                    })));
        }

    }
}
