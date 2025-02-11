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
    public class InstructionController : ControllerBase
    {
        private readonly IInstructionService _instructionService;

        public InstructionController(IInstructionService instructionService)
        {
            _instructionService = instructionService;
        }

        /// <summary>
        /// Returns a list of Instructions, each represents by a InstructionDto with their associated Dessert, Ingredient
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{InstructionDto}, {InstructionDto}, ...]
        /// </returns>
        /// <example>
        /// GET: api/Instruction/List
        /// </example>
        [HttpGet(template: "List")]
        public async Task<ActionResult<IEnumerable<InstructionDto>>> ListInstructions()
        {
            // returns a list of instruction dtos
            IEnumerable<InstructionDto> instructionDtos = await _instructionService.ListInstructions();
            return Ok(instructionDtos);
        }


        /// <summary>
        /// Returns a single Instruction specified by its {id}, represented by a Instruction Dto with its associated Dessert, Ingredient
        /// </summary>
        /// <param name="id">The instruction id</param>
        /// <returns>
        /// 200 OK
        /// {InstructionDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// GET: api/Instruction/Find/1 -> {InstructionDto}
        /// </example>
        [HttpGet(template: "Find/{id}")]
        public async Task<ActionResult<InstructionDto>> FindInstruction(int id)
        {
            // include will join the in(s)truction with 1 ingredient, 1 dessert
            // first or default async will get the first in(s)truction matching the {id}
            var instruction = await _instructionService.FindInstruction(id);

            // if the instruction could not be located, return 404 Not Found
            if (instruction == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(instruction);
            }

        }

        /// <summary>
        /// Updates a Instruction
        /// </summary>
        /// <param name="id">The ID of Instruction to update</param>
        /// <param name="instructionDto"></param>
        /// <returns>
        /// 400 Bad Request
        /// or
        /// 404 Not Found
        /// or
        /// 204 No Content
        /// </returns>
        /// <example>
        /// 
        /// </example>
        [HttpPut(template: "Update/{id}")]
        public async Task<ActionResult> UpdateInstruction(int id, InstructionDto instructionDto)
        {
            // {id} in URL must match OrderItemId in POST Body
            if (id != instructionDto.InstructionId)
            {
                //400 Bad Request
                return BadRequest();
            }

            ServiceResponse response = await _instructionService.UpdateInstruction(instructionDto);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            return NoContent();
        }

        /// <summary>
        /// Adds a Instruction
        /// </summary>
        /// <param name="instructionDto">The required information to add the instruction (ChangeIngredientOption,DessertId,IngredientId)</param>
        /// <returns>
        /// 201 Created
        /// Location: api/Instruction/Find/{InstructionId}
        /// {InstructionDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// POST: api/Instruction/Add
        /// </example>

        [HttpPost(template: "Add")]
        public async Task<ActionResult<Instruction>> AddInstruction(InstructionDto instructionDto)
        {
            ServiceResponse response = await _instructionService.AddInstruction(instructionDto);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            // returns 201 Created with Location
            return Created($"api/Instruction/Find/{response.CreatedId}", instructionDto);

        }

        /// <summary>
        /// Deletes the Instruction
        /// </summary>
        /// <param name="id">The id of the Instruction to delete</param>
        /// <returns>
        /// 201 No Content
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// 
        /// </example>
        [HttpDelete("/Delete/{id}")]
        public async Task<ActionResult> DeleteInstruction(int id)
        {
            ServiceResponse response = await _instructionService.DeleteInstruction(id);

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
        public async Task<IActionResult> ListInstructionsForDessert(int id)
        {
            // empty list of data transfer object InstructionDto
            IEnumerable<InstructionDto> instructionDtos = await _instructionService.ListInstructionsForDessert(id);
            // return 200 OK with InstructionDtos
            return Ok(instructionDtos);
        }

        [HttpGet(template: "ListForIngredient/{id}")]
        public async Task<IActionResult> ListInsturctionsForIngredient(int id)
        {
            // empty list of data transfer object OrderItemDto
            IEnumerable<InstructionDto> instructionDtos = await _instructionService.ListInstructionsForIngredient(id);
            // return 200 OK with InstructionDtos
            return Ok(instructionDtos);
        }
    }
}