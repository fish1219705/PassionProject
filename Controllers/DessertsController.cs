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
    public class DessertsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DessertsController(ApplicationDbContext context)
        {
            _context = context;
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
            List<Dessert> desserts = await _context.Desserts
                .Include(d => d.Ingredients)
                .Include(d => d.Reviews)
                .ToListAsync();

            List<DessertDto> DessertDtos = new List<DessertDto>();
            foreach(Dessert dessert in desserts)
            {
                DessertDtos.Add(new DessertDto()
                {
                    DessertId = dessert.DessertId,
                    DessertName = dessert.DessertName,
                    DessertDescription = dessert.DessertDescription,
                    SpecificTag = dessert.SpecificTag
                });

            }
            return Ok(DessertDtos);
        }

        /// <summary>
        /// Returns a single Dessert specified by its {id}, represented by a Dessert Dto with its associated Ingredients and Reviews
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

        [HttpGet(template:"Find/{id}")]
        public async Task<ActionResult<Dessert>> FindDessert(int id)
        {
           
            var dessert = await _context.Desserts
                .Include(d => d.Ingredients)
                .Include(d => d.Reviews)
                .FirstOrDefaultAsync(d => d.DessertId == id);

            if (dessert == null)
            {
                return NotFound();
            }
            DessertDto dessertDto = new DessertDto()
            {
                DessertId = dessert.DessertId,
                DessertName = dessert.DessertName,
                DessertDescription = dessert.DessertDescription,
                SpecificTag = dessert.SpecificTag
            };
            return  Ok(dessertDto);
        }

        // PUT: api/Desserts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut(template:"Update/{id}")]
        public async Task<IActionResult> UpdateDessert(int id, Dessert dessert)
        {
            if (id != dessert.DessertId)
            {
                return BadRequest();
            }

            _context.Entry(dessert).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DessertExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Desserts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost(template:"Add")]
        public async Task<ActionResult<Dessert>> AddDessert(Dessert dessert)
        {
            _context.Desserts.Add(dessert);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDessert", new { id = dessert.DessertId }, dessert);
        }

        // DELETE: api/Desserts/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteDessert(int id)
        {
            var dessert = await _context.Desserts.FindAsync(id);
            if (dessert == null)
            {
                return NotFound();
            }

            _context.Desserts.Remove(dessert);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DessertExists(int id)
        {
            return _context.Desserts.Any(e => e.DessertId == id);
        }

        
    }
}
