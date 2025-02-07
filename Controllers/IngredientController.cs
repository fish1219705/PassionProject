using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PassionProject.Data;
using PassionProject.Interfaces;
using PassionProject.Models;
using PassionProject.Services;

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
        /// Returns a list of Desserts, each represented by a DessertDto with their asscoiated Ingredients and Reviews
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{DeesertDto}, {DessertDto}, ...]
        /// </returns>
        /// <example>
        /// GET: api/Desserts/List -> [{DessertDto}, {DessertDto}, ...]
        /// </example>
        [HttpGet(template: "List")]
        public async Task<ActionResult<IEnumerable<IngredientDto>>> ListIngredients()
        {
            IEnumerable<IngredientDto> IngredientDtos = await _ingredientService.ListIngredients();

            return Ok(IngredientDtos);
        }


        /// <summary>
        /// Returns a single Dessert specified by its {id}
        /// </summary>
        /// <param name="id">The dessert id</param>
        /// <returns>
        /// 200 OK
        /// {DessertDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// GET: api/Desserts/Find/1 -> {DessertDto}
        /// </example>

        [HttpGet(template: "Find/{id}")]
        public async Task<ActionResult<IngredientDto>> FindIngredient(int id)
        {

            var ingredient = await _ingredientService.FindIngredient(id);

            // if the dessert could not be located, return 404 Not Found
            if (ingredient == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(ingredient);
            }
        }

        // PUT: api/Desserts/5

        [HttpPut(template: "Update/{id}")]
        public async Task<ActionResult> UpdateIngredient(int id, IngredientDto IngredientDto)
        {
            // {id} in URL must match DessertId in POST Body
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
            return Created($"api/Ingredient/FindIngredient/{response.CreatedId}", IngredientDto);
        }

        // POST: api/Desserts

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

        [HttpGet(template: "ListForDessert/{id}")]
        public async Task<IActionResult> ListIngredientsForDessert(int id)
        {
            // empty list of data transfer object IngredientDto
            IEnumerable<IngredientDto> IngredientDtos = await _ingredientService.ListIngredientsForDessert(id);
            // return 200 OK with Dtos
            return Ok(IngredientDtos);
        }

    }
}
