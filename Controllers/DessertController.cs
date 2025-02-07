using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PassionProject.Data;
using PassionProject.Models;
using PassionProject.Interfaces;

namespace PassionProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DessertController : ControllerBase
    {
        private readonly IDessertService _dessertService;
        // dependency injection of service interfaces
        public DessertController(IDessertService DessertService)
        {
            _dessertService = DessertService;
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
        [HttpGet(template:"List")]
        public async Task<ActionResult<IEnumerable<DessertDto>>> ListDesserts()
        {
            IEnumerable<DessertDto> DessertDtos = await _dessertService.ListDesserts();

            return Ok(DessertDtos);
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
        public async Task<ActionResult<DessertDto>> FindDessert(int id)
        {

            var dessert = await _dessertService.FindDessert(id);

            // if the dessert could not be located, return 404 Not Found
            if (dessert == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(dessert);
            }
        }

        // PUT: api/Desserts/5
       
        [HttpPut(template:"Update/{id}")]
        public async Task<ActionResult> UpdateDessert(int id, DessertDto DessertDto)
        {
               // {id} in URL must match DessertId in POST Body
                if (id != DessertDto.DessertId)
                {
                    //404 Bad Request
                    return BadRequest();
                }

                ServiceResponse response = await _dessertService.UpdateDessert(DessertDto);
                if(response.Status == ServiceResponse.ServiceStatus.NotFound)
                {
                    return NotFound(response.Messages);

                }
                else if(response.Status == ServiceResponse.ServiceStatus.Error)
                {
                    return StatusCode(500,response.Messages);
                }
                return Created($"api/Dessert/FindDessert/{response.CreatedId}", DessertDto);
            }

            // POST: api/Desserts
       
            [HttpPost(template:"Add")]
            public async Task<ActionResult<Dessert>> AddDessert(DessertDto DessertDto)
            {
                ServiceResponse response = await _dessertService.AddDessert(DessertDto);

                if (response.Status == ServiceResponse.ServiceStatus.NotFound)
                {
                return NotFound(response.Messages);
                }
                else if (response.Status == ServiceResponse.ServiceStatus.Error)
                {
                return StatusCode(500, response.Messages);
                }

                // returns 201 Created with Location
                return Created($"api/Dessert/FindDessert/{response.CreatedId}",DessertDto);
            }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> DeleteDessert(int id)
        {
            ServiceResponse response = await _dessertService.DeleteDessert(id);

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


        [HttpGet(template: "ListForIngredient/{id}")]
        public async Task<IActionResult> ListDessertsForIngredient(int id)
        {
            // empty list of data transfer object DessertDto
            IEnumerable<DessertDto> DessertDtos = await _dessertService.ListDessertsForIngredient(id);
            // return 200 OK with DessertDtos
            return Ok(DessertDtos);
        }

        /// <summary>
        /// Unlinks a dessert from a ingredient
        /// </summary>
        /// <param name="id">The id of the dessert</param>
        /// <param name="id">The id of the ingredient</param>
        /// <returns>
        /// 204 No Content
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// Delete: api/Dessert/Unlink?DessertId=4&IngredientId=12
        /// ->
        /// Response Code: 204 No Content
        /// </example>
        [HttpDelete("Unlink")]
        public async Task<ActionResult> Unlink(int dessertId, int ingredientId)
        {
            ServiceResponse response = await _dessertService.UnlinkDessertFromIngredient(dessertId, ingredientId);

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
        /// Links a dessert to a ingredient
        /// </summary>
        /// <param name="id">The id of the dessert</param>
        /// <param name="id">The id of the ingredient</param>
        /// <returns>
        /// 204 No Content
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// Post: api/Dessert/Link?DessertId=4&IngredientId=12
        /// ->
        /// Response Code: 204 No Content
        /// </example>
        [HttpPost("Link")]
        public async Task<ActionResult> Link(int dessertId, int ingredientId)
        {
            ServiceResponse response = await _dessertService.LinkDessertToIngredient(dessertId, ingredientId);

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
