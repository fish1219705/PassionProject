using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PassionProject.Data;
using PassionProject.Interfaces;
using PassionProject.Models;
using PassionProject.Services;
using PassionProject;

namespace PassionProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;
        // dependency injection of service interfaces
        public IngredientController(IIngredientService IngredientService)
        {
            _ingredientService = IngredientService;
        }

        /// <summary>
        /// Returns a list of Ingredients
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{IngredientDto}, {IngredientDto}, ...]
        /// </returns>
        /// <example>
        /// GET: api/Ingredient/List -> [{IngredientDto}, {IngredientDto}, ...]
        /// </example>
        [HttpGet(template: "List")]
        public async Task<ActionResult<IEnumerable<IngredientDto>>> ListIngredients()
        {
            // empty list of data transfer object IngredientDto
            IEnumerable<IngredientDto> IngredientDtos = await _ingredientService.ListIngredients();
            // return 200 OK with IngredientDtos
            return Ok(IngredientDtos);
        }


        /// <summary>
        /// Returns a single Ingredient specified by its {id}
        /// </summary>
        /// <param name="id">The Ingredient id</param>
        /// <returns>
        /// 200 OK
        /// {IngredientDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// GET: api/Ingredient/Find/1 -> {IngredientDto}
        /// </example>
        [HttpGet(template: "Find/{id}")]
        public async Task<ActionResult<IngredientDto>> FindIngredient(int id)
        {

            var Ingredient = await _ingredientService.FindIngredient(id);

            // if the Ingredient could not be located, return 404 Not Found
            if (Ingredient == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(Ingredient);
            }
        }

        // PUT: api/Desserts/5

        /// <summary>
        /// Updates a Ingredient
        /// </summary>
        /// <param name="id">The ID of the Ingredient to update</param>
        /// <param name="IngredientDto">The required information to update the Ingredient (IngredientName, IngredientDescription)</param>
        /// <returns>
        /// 400 Bad Request
        /// or
        /// 404 Not Found
        /// or
        /// 204 No Content
        /// </returns>
        /// <example>
        /// PUT: api/Ingredient/Update/3
        /// Request Headers: Content-Type: application/json
        /// Request Body: {IngredientDto}
        /// ->
        /// Response Code: 204 No Content
        /// </example>
        [HttpPut(template: "Update/{id}")]
        public async Task<ActionResult> UpdateIngredient(int id, IngredientDto IngredientDto)
        {
            // {id} in URL must match IngredientId in POST Body
            if (id != IngredientDto.IngredientId)
            {
                //404 Bad Request
                return BadRequest();
            }

            ServiceResponse response = await _ingredientService.UpdateIngredient(IngredientDto);
            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);

            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            //Status = Updated
            return NoContent();
        }


        /// <summary>
        /// Adds a Ingredient
        /// </summary>
        /// <param name="IngredientDto">The required information to add the Ingredient (IngredientName, IngredientDescription)</param>
        /// <returns>
        /// 201 Created
        /// Location: api/Ingredient/Find/{IngredientId}
        /// {IngredientDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// POST: api/Ingredient/Add
        /// Request Headers: Content-Type: application/json
        /// Request Body: {IngredientDto}
        /// ->
        /// Response Code: 201 Created
        /// Response Headers: Location: api/Ingredient/Find/{IngredientId}
        /// </example>
        [HttpPost(template: "Add")]
        public async Task<ActionResult<Ingredient>> AddIngredient(IngredientDto IngredientDto)
        {
            ServiceResponse response = await _ingredientService.AddIngredient(IngredientDto);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            // returns 201 Created with Location
            return Created($"api/Ingredient/FindIngredient/{response.CreatedId}", IngredientDto);
        }

        /// <summary>
        /// Deletes the Ingredient
        /// </summary>
        /// <param name="id">The id of the Ingredeint to delete</param>
        /// <returns>
        /// 204 No Content
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// DELETE: api/Ingredeint/Delete/3
        /// ->
        /// Response Code: 204 No Content
        /// </example>
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> DeleteIngredient(int id)
        {
            ServiceResponse response = await _ingredientService.DeleteIngredient(id);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound();
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            return NoContent();

        }

        /// <summary>
        /// Returns a list of ingredients for a specific dessert by its {id}
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{IngredientDto},{IngredientDto},..]
        /// </returns>
        /// <example>
        /// GET: api/Ingredient/ListForDessert/3 -> [{IngredientDto},{IngredientDto},..]
        /// </example>
        [HttpGet(template: "ListForDessert/{id}")]
        public async Task<IActionResult> ListIngredientsForDessert(int id)
        {
            // empty list of data transfer object IngredientDto
            IEnumerable<IngredientDto> IngredientDtos = await _ingredientService.ListIngredientsForDessert(id);
            // return 200 OK with IngredientDtos
            return Ok(IngredientDtos);
        }

        /// <summary>
        /// Unlinks a ingredient from a dessert
        /// </summary>
        /// <param name="ingredientId">The id of the ingredient</param>
        /// <param name="dessertId">The id of the dessert</param>
        /// <returns>
        /// 204 No Content
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// Delete: api/Ingredient/Unlink?IngredientId=1&DessertId=1
        /// ->
        /// Response Code: 204 No Content
        /// </example>
        [HttpDelete("Unlink")]
        public async Task<ActionResult> Unlink(int ingredientId, int dessertId)
        {
            ServiceResponse response = await _ingredientService.UnlinkIngredientFromDessert(ingredientId, dessertId);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound();
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            return NoContent();

        }

        /// <summary>
        /// Links a ingredient to a dessert
        /// </summary>
        /// <param name="ingredientId">The id of the ingredient</param>
        /// <param name="dessertId">The id of the dessert</param>
        /// <returns>
        /// 204 No Content
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// Post: api/Ingredient/Link?IngredientId=1&DessertId=1
        /// ->
        /// Response Code: 204 No Content
        /// </example>
        [HttpPost("Link")]
        public async Task<ActionResult> Link(int ingredientId, int dessertId)
        {
            ServiceResponse response = await _ingredientService.LinkIngredientToDessert(ingredientId, dessertId);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound();
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            return NoContent();

        }

    }
}
