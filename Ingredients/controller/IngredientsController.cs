using Microsoft.AspNetCore.Mvc;
using StoreApp.Exceptions;
using StoreApp.Ingredients.Models;

namespace StoreApp.Ingredients
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientsController(IIngredientService ingredientsService) : ControllerBase
    {
        private readonly IIngredientService _ingredientsService = ingredientsService;


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var ingredients = await _ingredientsService.GetAll();
                return Ok(ingredients);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while retrieving ingredients", details = ex.Message });
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {

                var ingredient = await _ingredientsService.GetById(id);
                if (ingredient == null)
                {
                    return NotFound(new { error = $"Ingredient with id {id} not found" });
                }
                return Ok(ingredient);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while retrieving the ingredient", details = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateIngredient([FromBody] IngredientSaveDto ingredientPayload)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var ingredient = await _ingredientsService.AddOne(ingredientPayload);
                return CreatedAtAction(nameof(GetById), new { id = ingredient.Id }, ingredient);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while creating the ingredient", details = ex.Message });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIngredient(int id, [FromBody] IngredientSaveDto ingredientPayload)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _ingredientsService.UpdateOne(id, ingredientPayload);
                return Ok("Ingredient updated successfully");
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
                return StatusCode(500, new { error = "An error occurred while updating the ingredient", details = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngredient(int id)
        {
            try
            {

                await _ingredientsService.DeleteOne(id);
                return Ok("Ingredient deleted successfully");
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
                return StatusCode(500, new { error = "An error occurred while deleting the ingredient", details = ex.Message });
            }
        }


        [HttpPatch("{id}/stock")]
        public async Task<IActionResult> UpdateInStock(int id, [FromBody] BulkInStockUpdateDto payload)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var ingredient = await _ingredientsService.UpdateInStock(id, payload.InStock);
                return Ok(ingredient);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while updating ingredient stock status", details = ex.Message });
            }
        }


        [HttpPatch("stock/bulk")]
        public async Task<IActionResult> UpdateAllInStock([FromBody] BulkInStockUpdateDto payload)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _ingredientsService.UpdateAllInStock(payload.InStock);
                return Ok("All ingredients stock status updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while updating all ingredients stock status", details = ex.Message });
            }
        }
    }
}


