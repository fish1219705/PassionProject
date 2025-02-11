using PassionProject.Interfaces;
using PassionProject.Models;
using PassionProject.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace PassionProject.Services
{
    public class InstructionService : IInstructionService
    {
        private readonly ApplicationDbContext _context;
        // dependency injection of database context
        public InstructionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InstructionDto>> ListInstructions()
        {
            // include will join the in(s)truction with 1 dessert, 1 ingredient
            List<Instruction> instructions = await _context.Instructions
                .Include(s => s.Dessert)
                .Include(s => s.Ingredient)
                .ToListAsync();
            // empty list of data transfer object InstructionDto
            List<InstructionDto> instructionDtos = new List<InstructionDto>();
            // foreach Instruction record in database
            foreach (Instruction instruction in instructions)
            {
                // create nw instance of InstructionDto, add to list
                instructionDtos.Add(new InstructionDto()
                {
                    InstructionId = instruction.InstructionId,
                    ChangeIngredientOption = instruction.ChangeIngredientOption,
                    QtyOfIngredient = instruction.QtyOfIngredient,
                    DessertId = instruction.DessertId,
                    DessertName = instruction.Dessert.DessertName,
                    IngredientId = instruction.IngredientId,
                    IngredientName = instruction.Ingredient.IngredientName
                });
            }
            return instructionDtos;
        }

        public async Task<InstructionDto?> FindInstruction(int id)
        {
            var instruction = await _context.Instructions
                .Include(s => s.Dessert)
                .Include(s => s.Ingredient)
                .FirstOrDefaultAsync(s => s.InstructionId == id);

            if (instruction == null)
            {
                return null;
            }

            InstructionDto instructionDto = new InstructionDto()
            {
                InstructionId = instruction.InstructionId,
                ChangeIngredientOption = instruction.ChangeIngredientOption,
                QtyOfIngredient = instruction.QtyOfIngredient,
                DessertId = instruction.DessertId,
                DessertName = instruction.Dessert.DessertName,
                IngredientId = instruction.IngredientId,
                IngredientName = instruction.Ingredient.IngredientName
            };
            return instructionDto;
        }

        public async Task<ServiceResponse> UpdateInstruction(InstructionDto instructionDto)
        {
            ServiceResponse serviceResponse = new();
            Dessert? dessert = await _context.Desserts.FindAsync(instructionDto.DessertId);
            Ingredient? ingredient = await _context.Ingredients.FindAsync(instructionDto.IngredientId);

            if (dessert == null || ingredient == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                return serviceResponse;
            }

            Instruction instruction = new Instruction()
            {
                InstructionId = Convert.ToInt32(instructionDto.InstructionId),
                ChangeIngredientOption = instructionDto.ChangeIngredientOption,
                QtyOfIngredient = instructionDto.QtyOfIngredient,
                DessertId = instructionDto.DessertId,
                Dessert = dessert,
                IngredientId = instructionDto.IngredientId,
                Ingredient = ingredient

            };
            _context.Entry(instruction).State = EntityState.Modified;
            try
            {
                // SQL Equivalent: Update Instructions set ... where InstructionId={id}
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("An error occurred updating the record");
                return serviceResponse;
            }


            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;

            return serviceResponse;
        }
        public async Task<ServiceResponse> AddInstruction(InstructionDto instructionDto)
        {
            ServiceResponse serviceResponse = new();
            Dessert? dessert = await _context.Desserts.FindAsync(instructionDto.DessertId);
            Ingredient? ingredient = await _context.Ingredients.FindAsync(instructionDto.IngredientId);

            if (dessert == null || ingredient == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                if (dessert == null)
                {
                    serviceResponse.Messages.Add("Dessert was not found. ");
                }
                if (ingredient == null)
                {
                    serviceResponse.Messages.Add("Ingredient was not found.");
                }
                return serviceResponse;
            }

            Instruction instruction = new Instruction()
            {
                ChangeIngredientOption = instructionDto.ChangeIngredientOption,
                QtyOfIngredient = instructionDto.QtyOfIngredient,
                DessertId = instructionDto.DessertId,
                Dessert = dessert,
                IngredientId = instructionDto.IngredientId,
                Ingredient = ingredient
            };
            try
            {
                _context.Instructions.Add(instruction);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("There was an error adding the Instruction.");
                serviceResponse.Messages.Add(ex.Message);
            }


            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = instruction.InstructionId;
            return serviceResponse;
        }

        public async Task<ServiceResponse> DeleteInstruction(int id)
        {
            ServiceResponse response = new();
            var instruction = await _context.Instructions.FindAsync(id);
            if (instruction == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Instruction cannot be deleted because it does not exist.");
                return response;
            }

            try
            {
                _context.Instructions.Remove(instruction);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error encountered while deleting instruction");
                return response;
            }

            response.Status = ServiceResponse.ServiceStatus.Deleted;

            return response;
        }

        public async Task<IEnumerable<InstructionDto>> ListInstructionsForDessert(int id)
        {
            List<Instruction> instructions = await _context.Instructions
                .Include(s => s.Dessert)
                .Include(s => s.Ingredient)
                .Where(s => s.DessertId == id)
                .ToListAsync();
            List<InstructionDto> instructionDtos = new List<InstructionDto>();
            foreach (Instruction instruction in instructions)
            {
                instructionDtos.Add(new InstructionDto()
                {
                    InstructionId = instruction.InstructionId,
                    ChangeIngredientOption = instruction.ChangeIngredientOption,
                    QtyOfIngredient = instruction.QtyOfIngredient,
                    DessertId = instruction.DessertId,
                    DessertName = instruction.Dessert.DessertName,
                    IngredientId = instruction.IngredientId,
                    IngredientName = instruction.Ingredient.IngredientName

                });
            }
            return instructionDtos;

        }

        public async Task<IEnumerable<InstructionDto>> ListInstructionsForIngredient(int id)
        {
            List<Instruction> instructions = await _context.Instructions
                .Include(s => s.Dessert)
                .Include(s => s.Ingredient)
                .Where(s => s.IngredientId == id)
                .ToListAsync();

            List<InstructionDto> instructionDtos = new List<InstructionDto>();
            foreach (Instruction instruction in instructions)
            {
                instructionDtos.Add(new InstructionDto()
                {
                    InstructionId = instruction.InstructionId,
                    ChangeIngredientOption = instruction.ChangeIngredientOption,
                    QtyOfIngredient = instruction.QtyOfIngredient,
                    DessertId = instruction.DessertId,
                    DessertName = instruction.Dessert.DessertName,
                    IngredientId = instruction.IngredientId,
                    IngredientName = instruction.Ingredient.IngredientName

                });
            }
            return instructionDtos;

        }
    }
}

