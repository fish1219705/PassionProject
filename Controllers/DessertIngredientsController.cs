using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PassionProject.Data;
using PassionProject.Models;

namespace PassionProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DessertIngredientsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DessertIngredientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of Dessert Ingredients, each represents by a DessertIngredientDto with their associated Dessert, Ingredient
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{DessertIngredientDto}, {DessertIngredientDto}, ...]
        /// </returns>
        /// <example>
        /// GET: api/DessertIngredients/List
        /// </example>
        [HttpGet(template: "List")]
        public async Task<ActionResult<IEnumerable<DessertIngredient>>> ListDessertIngredients()
        {
            // include will join the dessert(i)ngredient with 1 ingredient, 1 dessert
            List<DessertIngredient> dessertIngredients = await _context.DessertIngredient
                .Include(i => i.Ingredient)
                .Include(i => i.Dessert)
                .ToListAsync();
            // empty list of data transfer objects DessertIngredientDto
            List<DessertIngredientDto> dessertIngredientDtos = new List<DessertIngredientDto>();
            // foreach Dessert Ingredient record in the database
            foreach (DessertIngredient dessertIngredient in dessertIngredients)
            {
                // create new instance of DessertIngredientDto, add to list
                dessertIngredientDtos.Add(new DessertIngredientDto()
                {
                    DessertIngredientId = dessertIngredient.DessertIngredientId,
                    ChangeIngredientOption = dessertIngredient.ChangeIngredientOption,
                    IngredientId = dessertIngredient.IngredientId,
                    DessertId = dessertIngredient.DessertId,
                    IngredientName = dessertIngredient.Ingredient.IngredientName,
                    DessertName = dessertIngredient.Dessert.DessertName

                });
            }
            // return 200 OK with DessertIngredientDtos
            return Ok(dessertIngredientDtos);
        }


        /// <summary>
        /// Returns a single Dessert Ingredient specified by its {id}, represented by a Dessert Ingredient Dto with its associated Dessert, Ingredient
        /// </summary>
        /// <param name="id">The dessert ingredient id</param>
        /// <returns>
        /// 200 OK
        /// {DessertIngredientDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// GET: api/DessertIngredients/Find/1 -> {DessertIngredientDto}
        /// </example>
        [HttpGet(template: "Find/{id}")]
        public async Task<ActionResult<DessertIngredient>> FindDessertIngredient(int id)
        {
            // include will join the dessert(i)ngredient with 1 ingredient, 1 dessert
            // first or default async will get the first dessert(i)ngredient matching the {id}
            var dessertIngredient = await _context.DessertIngredient
                .Include(i => i.Ingredient)
                .Include(i => i.Dessert)
                .FirstOrDefaultAsync(i => i.DessertIngredientId == id);

            // if the item could not be located, return 404 Not Found
            if (dessertIngredient == null)
            {
                return NotFound();
            }
            // create an instance of DessertIngredientDto
            DessertIngredientDto dessertIngredientDto = new DessertIngredientDto()
            {
                DessertIngredientId = dessertIngredient.DessertIngredientId,
                ChangeIngredientOption = dessertIngredient.ChangeIngredientOption,
                IngredientId = dessertIngredient.IngredientId,
                DessertId = dessertIngredient.DessertId,
                IngredientName = dessertIngredient.Ingredient.IngredientName,
                DessertName = dessertIngredient.Dessert.DessertName
            };
            //return 200 OK with dessertIngredientDto
            return Ok(dessertIngredientDto);
        }

        /// <summary>
        /// Updates a Dessert Ingredient
        /// </summary>
        /// <param name="id">The ID of Dessert Ingredient to update</param>
        /// <param name="dessertIngredientDto"></param>
        /// <returns>
        /// 400 Bad Request
        /// or
        /// 404 Not Found
        /// or
        /// 204 No Content
        /// </returns>
        /// <example>
        /// PUT: api/DessertIngredients/Update/1
        /// </example>

        [HttpPut(template: "Update/{id}")]
        public async Task<IActionResult> UpdateDessertIngredient(int id, DessertIngredientDto dessertIngredientDto)
        {
            // {id} in URL must match OrderItemId in POST Body
            if (id != dessertIngredientDto.DessertIngredientId)
            {
                //400 Bad Request
                return BadRequest();
            }
            // attempt to find associated Ingredient, Dessert in DB by looking up IngredientId and DessertId foreign keys
            var ingredient = await _context.Ingredients.FindAsync(dessertIngredientDto.IngredientId);
            var dessert = await _context.Desserts.FindAsync(dessertIngredientDto.DessertId);
            // Posted data must link to valid entity
            if (ingredient == null || dessert == null)
            {
                //404 Not Found
                return NotFound();
            }
            // Create instance of DessertIngredient
            DessertIngredient dessertIngredient = new DessertIngredient()
            {
                DessertIngredientId = Convert.ToInt32(dessertIngredientDto.DessertIngredientId),
                ChangeIngredientOption = dessertIngredientDto.ChangeIngredientOption,
                IngredientId = dessertIngredientDto.IngredientId,
                DessertId = dessertIngredientDto.DessertId,
                Ingredient = ingredient,
                Dessert = dessert
            };
            // flags that the object has changed
            _context.Entry(dessertIngredient).State = EntityState.Modified;

            try
            {
                // SQL Equivalent: UPDATE DessertIngredients SET ... WHERE DessertIngredientId = {id}
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DessertIngredientExists(id))
                {
                    //404 Not Found
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        /// <summary>
        /// Adds a Dessert Ingredient
        /// </summary>
        /// <param name="dessertIngredientDto">The required information to add the dessert ingredient (ChangeIngredientOption,DessertId,IngredientId)</param>
        /// <returns>
        /// 201 Created
        /// Location: api/DessertIngredients/Find/{DessertIngredientId}
        /// {DessertIngredientDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// POST: api/DessertIngredients/Add
        /// </example>

        [HttpPost(template: "Add")]
        public async Task<ActionResult<DessertIngredient>> AddDessertIngredient(DessertIngredient dessertIngredientDto)
        {

            // attempt to find associated Ingredient, Dessert in DB by looking up IngredientId and DessertId foreign keys
            var ingredient = await _context.Ingredients.FindAsync(dessertIngredientDto.IngredientId);
            var dessert = await _context.Desserts.FindAsync(dessertIngredientDto.DessertId);
            // Posted data must link to valid entity
            if (ingredient == null || dessert == null)
            {
                //404 Not Found
                return NotFound();
            }

            DessertIngredient dessertIngredient = new DessertIngredient()
            {
                DessertIngredientId = Convert.ToInt32(dessertIngredientDto.DessertIngredientId),
                ChangeIngredientOption = dessertIngredientDto.ChangeIngredientOption,
                IngredientId = dessertIngredientDto.IngredientId,
                DessertId = dessertIngredientDto.DessertId,
                Ingredient = ingredient,
                Dessert = dessert
            };
            // SQL Equivalent: INSERT INTO DessertIngredients VALUES (...)
            _context.DessertIngredient.Add(dessertIngredient);
            await _context.SaveChangesAsync();

            // returns 201 Created with Location
            return CreatedAtAction("GetDessertIngredient", new { id = dessertIngredient.DessertIngredientId }, dessertIngredientDto);
        }

        /// <summary>
        /// Deletes the Dessert Ingredient
        /// </summary>
        /// <param name="id">The id of the Dessert Ingredient to delete</param>
        /// <returns>
        /// 201 No Content
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// DELETE: api/DessertIngredients/Delete/1
        /// </example>
        [HttpDelete("/Delete/{id}")]
        public async Task<IActionResult> DeleteDessertIngredient(int id)
        {
            var dessertIngredient = await _context.DessertIngredient.FindAsync(id);
            if (dessertIngredient == null)
            {
                return NotFound();
            }

            _context.DessertIngredient.Remove(dessertIngredient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DessertIngredientExists(int id)
        {
            return _context.DessertIngredient.Any(e => e.DessertIngredientId == id);
        }

        [HttpGet(template: "ListForIngredient")]
        public async Task<IEnumerable<DessertIngredientDto>> ListDIForIngredient(int id)
        {

            List<DessertIngredient> dessertIngredients = await _context.DessertIngredient
                .Include(i => i.Ingredient)
                .Include(i => i.Dessert)
                .Where(i => i.IngredientId == id)
                .ToListAsync();

            // empty list of data transfer object DessertIngredientDto
            List<DessertIngredientDto> dessertIngredientDtos = new List<DessertIngredientDto>();
            // foreach Dessert Ingredient record in database
            foreach (DessertIngredient dessertIngredient in dessertIngredients)
            {
                // create new instance of DessertDto, add to list
                dessertIngredientDtos.Add(new DessertIngredientDto()
                {
                    DessertIngredientId = dessertIngredient.DessertIngredientId,
                    ChangeIngredientOption = dessertIngredient.ChangeIngredientOption,
                    IngredientId = dessertIngredient.IngredientId,
                    DessertId = dessertIngredient.DessertId,
                    DessertName = dessertIngredient.Dessert.DessertName,
                    IngredientName = dessertIngredient.Ingredient.IngredientName
                });

            }
            // return DessertDtos
            return dessertIngredientDtos;
        }

        [HttpGet(template: "ListForDessert")]
        public async Task<IEnumerable<DessertIngredientDto>> ListDIForDessert(int id)
        {

            List<DessertIngredient> dessertIngredients = await _context.DessertIngredient
                .Include(i => i.Ingredient)
                .Include(i => i.Dessert)
                .Where(i => i.DessertId == id)
                .ToListAsync();

            // empty list of data transfer object DessertIngredientDto
            List<DessertIngredientDto> dessertIngredientDtos = new List<DessertIngredientDto>();
            // foreach Dessert Ingredient record in database
            foreach (DessertIngredient dessertIngredient in dessertIngredients)
            {
                // create new instance of DessertDto, add to list
                dessertIngredientDtos.Add(new DessertIngredientDto()
                {
                    DessertIngredientId = dessertIngredient.DessertIngredientId,
                    ChangeIngredientOption = dessertIngredient.ChangeIngredientOption,
                    IngredientId = dessertIngredient.IngredientId,
                    DessertId = dessertIngredient.DessertId,
                    DessertName = dessertIngredient.Dessert.DessertName,
                    IngredientName = dessertIngredient.Ingredient.IngredientName
                });

            }
            // return DessertDtos
            return dessertIngredientDtos;
        }
    }
}
