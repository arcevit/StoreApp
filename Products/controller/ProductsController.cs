using Microsoft.AspNetCore.Mvc;
using StoreApp.Exceptions;
using StoreApp.Products.Models;

namespace StoreApp.Products
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IProductService productsService) : ControllerBase
    {
        private readonly IProductService _productsService = productsService;


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var products = await _productsService.GetAll();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while retrieving products", details = ex.Message });
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {

                var product = await _productsService.GetById(id);
                if (product == null)
                {
                    return NotFound(new { error = $"Product with id {id} not found" });
                }
                return Ok(product);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while retrieving the product", details = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductSaveDto productPayload)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdProduct = await _productsService.AddOne(productPayload);
                return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while creating the product", details = ex.Message });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductSaveDto productPayload)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _productsService.UpdateOne(id, productPayload);
                return Ok("Product updated successfully");
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while updating the product", details = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productsService.DeleteOne(id);
                return Ok("Product deleted successfully");
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while deleting the product", details = ex.Message });
            }
        }


        [HttpPost("{productId}/ingredients/{ingredientId}")]
        public async Task<IActionResult> AddIngredient(int productId, int ingredientId)
        {
            try
            {

                var product = await _productsService.AddIngredient(productId, ingredientId);
                return Ok(product);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while adding ingredient to product", details = ex.Message });
            }
        }


        [HttpDelete("{productId}/ingredients/{ingredientId}")]
        public async Task<IActionResult> RemoveIngredient(int productId, int ingredientId)
        {
            try
            {

                var product = await _productsService.RemoveIngredient(productId, ingredientId);
                return Ok(product);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while removing ingredient from product", details = ex.Message });
            }
        }
    }
}
