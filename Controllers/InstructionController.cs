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
    public class InstructionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InstructionController(ApplicationDbContext context)
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
        public async Task<ActionResult<IEnumerable<InstructionDto>>> ListInstructions()
        {
            // include will join the dessert(i)ngredient with 1 ingredient, 1 dessert
            List<Instruction> instructions = await _context.Instructions
                .Include(i => i.Ingredient)
                .Include(i => i.Dessert)
                .ToListAsync();
            // empty list of data transfer objects DessertIngredientDto
            List<InstructionDto> instructionDtos = new List<InstructionDto>();
            // foreach Dessert Ingredient record in the database
            foreach (Instruction instruction in instructions)
            {
                // create new instance of DessertIngredientDto, add to list
                instructionDtos.Add(new InstructionDto()
                {
                    InstructionId = instruction.InstructionId,
                    ChangeIngredientOption = instruction.ChangeIngredientOption,
                    IngredientId = instruction.IngredientId,
                    DessertId = instruction.DessertId,
                    IngredientName = instruction.Ingredient.IngredientName,
                    DessertName = instruction.Dessert.DessertName

                });
            }
            // return 200 OK with DessertIngredientDtos
            return Ok(instructionDtos);
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
        public async Task<ActionResult<InstructionDto?>> FindInstruction(int id)
        {
            // include will join the dessert(i)ngredient with 1 ingredient, 1 dessert
            // first or default async will get the first dessert(i)ngredient matching the {id}
            var instruction = await _context.Instructions
                .Include(i => i.Ingredient)
                .Include(i => i.Dessert)
                .FirstOrDefaultAsync(i => i.InstructionId == id);

            // if the item could not be located, return 404 Not Found
            if (instruction == null)
            {
                return NotFound();
            }
            // create an instance of DessertIngredientDto
            InstructionDto instructionDto = new InstructionDto()
            {
                InstructionId = instruction.InstructionId,
                QtyOfIngredient = instruction.QtyOfIngredient,
                ChangeIngredientOption = instruction.ChangeIngredientOption,
                IngredientId = instruction.IngredientId,
                DessertId = instruction.DessertId,
                IngredientName = instruction.Ingredient.IngredientName,
                DessertName = instruction.Dessert.DessertName
            };
            //return 200 OK with dessertIngredientDto
            return Ok(instructionDto);
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
        public async Task<IActionResult> UpdateInstruction(int id, InstructionDto instructionDto)
        {
            // {id} in URL must match OrderItemId in POST Body
            if (id != instructionDto.InstructionId)
            {
                //400 Bad Request
                return BadRequest();
            }
            // attempt to find associated Ingredient, Dessert in DB by looking up IngredientId and DessertId foreign keys
            var ingredient = await _context.Ingredients.FindAsync(instructionDto.IngredientId);
            var dessert = await _context.Desserts.FindAsync(instructionDto.DessertId);
            // Posted data must link to valid entity
            if (ingredient == null || dessert == null)
            {
                //404 Not Found
                return NotFound();
            }
            // Create instance of DessertIngredient
            Instruction instruction = new Instruction()
            {
                InstructionId = Convert.ToInt32(instructionDto.InstructionId),
                ChangeIngredientOption = instructionDto.ChangeIngredientOption,
                QtyOfIngredient = instructionDto.QtyOfIngredient,
                IngredientId = instructionDto.IngredientId,
                DessertId = instructionDto.DessertId,
                Ingredient = ingredient,
                Dessert = dessert
            };
            // flags that the object has changed
            _context.Entry(instruction).State = EntityState.Modified;

            try
            {
                // SQL Equivalent: UPDATE DessertIngredients SET ... WHERE DessertIngredientId = {id}
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstructionExists(id))
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
        public async Task<ActionResult<Instruction>> AddInstruction(Instruction instructionDto)
        {

            // attempt to find associated Ingredient, Dessert in DB by looking up IngredientId and DessertId foreign keys
            var ingredient = await _context.Ingredients.FindAsync(instructionDto.IngredientId);
            var dessert = await _context.Desserts.FindAsync(instructionDto.DessertId);
            // Posted data must link to valid entity
            if (ingredient == null || dessert == null)
            {
                //404 Not Found
                return NotFound();
            }

            Instruction instruction = new Instruction()
            {
                InstructionId = Convert.ToInt32(instructionDto.InstructionId),
                ChangeIngredientOption = instructionDto.ChangeIngredientOption,
                IngredientId = instructionDto.IngredientId,
                DessertId = instructionDto.DessertId,
                Ingredient = ingredient,
                Dessert = dessert
            };
            // SQL Equivalent: INSERT INTO DessertIngredients VALUES (...)
            _context.Instructions.Add(instruction);
            await _context.SaveChangesAsync();

            // returns 201 Created with Location
            return CreatedAtAction("GeTInstruction", new { id = instruction.InstructionId }, instructionDto);
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
        public async Task<IActionResult> DeleteInstruction(int id)
        {
            var instruction = await _context.Instructions.FindAsync(id);
            if (instruction == null)
            {
                return NotFound();
            }

            _context.Instructions.Remove(instruction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InstructionExists(int id)
        {
            return _context.Instructions.Any(e => e.InstructionId == id);
        }

        //[HttpGet(template: "ListForIngredient")]
        //public async Task<IEnumerable<DessertIngredientDto>> ListDIForIngredient(int id)
        //{

        //    List<Instruction> dessertIngredients = await _context.DessertIngredient
        //        .Include(i => i.Ingredient)
        //        .Include(i => i.Dessert)
        //        .Where(i => i.IngredientId == id)
        //        .ToListAsync();

        //    // empty list of data transfer object DessertIngredientDto
        //    List<DessertIngredientDto> dessertIngredientDtos = new List<DessertIngredientDto>();
        //    // foreach Dessert Ingredient record in database
        //    foreach (Instruction dessertIngredient in dessertIngredients)
        //    {
        //        // create new instance of DessertDto, add to list
        //        dessertIngredientDtos.Add(new DessertIngredientDto()
        //        {
        //            DessertIngredientId = dessertIngredient.DessertIngredientId,
        //            ChangeIngredientOption = dessertIngredient.ChangeIngredientOption,
        //            IngredientId = dessertIngredient.IngredientId,
        //            DessertId = dessertIngredient.DessertId,
        //            DessertName = dessertIngredient.Dessert.DessertName,
        //            IngredientName = dessertIngredient.Ingredient.IngredientName
        //        });

        //    }
        //    // return DessertDtos
        //    return dessertIngredientDtos;
        //}

        //[HttpGet(template: "ListForDessert")]
        //public async Task<IEnumerable<DessertIngredientDto>> ListDIForDessert(int id)
        //{

        //    List<Instruction> dessertIngredients = await _context.DessertIngredient
        //        .Include(i => i.Ingredient)
        //        .Include(i => i.Dessert)
        //        .Where(i => i.DessertId == id)
        //        .ToListAsync();

        //    // empty list of data transfer object DessertIngredientDto
        //    List<DessertIngredientDto> dessertIngredientDtos = new List<DessertIngredientDto>();
        //    // foreach Dessert Ingredient record in database
        //    foreach (Instruction dessertIngredient in dessertIngredients)
        //    {
        //        // create new instance of DessertDto, add to list
        //        dessertIngredientDtos.Add(new DessertIngredientDto()
        //        {
        //            DessertIngredientId = dessertIngredient.DessertIngredientId,
        //            ChangeIngredientOption = dessertIngredient.ChangeIngredientOption,
        //            IngredientId = dessertIngredient.IngredientId,
        //            DessertId = dessertIngredient.DessertId,
        //            DessertName = dessertIngredient.Dessert.DessertName,
        //            IngredientName = dessertIngredient.Ingredient.IngredientName
        //        });

        //    }
        //    // return DessertDtos
        //    return dessertIngredientDtos;
        //}
    }
}
