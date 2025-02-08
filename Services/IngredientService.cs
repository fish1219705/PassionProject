using PassionProject.Interfaces;
using PassionProject.Models;
using PassionProject.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace PassionProject.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly ApplicationDbContext _context;
        // dependency injection of database context
        public IngredientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<IngredientDto>> ListIngredients()
        {
            // all Ingredients
            List<Ingredient> Ingredients = await _context.Ingredients
                .ToListAsync();
            // empty list of data transfer object IngredientDto
            List<IngredientDto> IngredientDtos = new List<IngredientDto>();
            // foreach Ingredient record in database
            foreach (Ingredient Ingredient in Ingredients)
            {
                // create new instance of IngredientDto, add to list
                IngredientDtos.Add(new IngredientDto()
                {
                    IngredientId = Ingredient.IngredientId,
                    IngredientName = Ingredient.IngredientName,
                    IngredientDescription = Ingredient.IngredientDescription

                });
            }
            // return IngredientDtos
            return IngredientDtos;

        }

        public async Task<IngredientDto?> FindIngredient(int id)
        {
            // first or default async will get the first (i)ngredient matching the {id}
            var Ingredient = await _context.Ingredients
                .FirstOrDefaultAsync(i => i.IngredientId == id);

            // no ingredient found
            if (Ingredient == null)
            {
                return null;
            }
            // create an instance of IngredientDto
            IngredientDto IngredientDto = new IngredientDto()
            {
                IngredientId = Ingredient.IngredientId,
                IngredientName = Ingredient.IngredientName,
                IngredientDescription = Ingredient.IngredientDescription
            };
            return IngredientDto;

        }

        public async Task<ServiceResponse> UpdateIngredient(IngredientDto IngredientDto)
        {
            ServiceResponse serviceResponse = new();

            // Create instance of Ingredient
            Ingredient Ingredient = new Ingredient()
            {
                IngredientId = IngredientDto.IngredientId,
                IngredientName = IngredientDto.IngredientName,
                IngredientDescription = IngredientDto.IngredientDescription
            };
            // flags that the object has changed
            _context.Entry(Ingredient).State = EntityState.Modified;

            try
            {
                //SQL Equivalent: Update Ingredients set ... where IngredientId={id}
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("An error occured updating the record");
                return serviceResponse;
            }
            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            return serviceResponse;
        }

        public async Task<ServiceResponse> AddIngredient(IngredientDto IngredientDto)
        {
            ServiceResponse serviceResponse = new();


            // Create instance of Ingredient
            Ingredient Ingredient = new Ingredient()
            {
                IngredientName = IngredientDto.IngredientName,
                IngredientDescription = IngredientDto.IngredientDescription,
            };
            // SQL Equivalent: Insert into Ingredients (..) values (..)

            try
            {
                _context.Ingredients.Add(Ingredient);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("There was an error adding the Ingredient.");
                serviceResponse.Messages.Add(ex.Message);

            }


            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = Ingredient.IngredientId;
            return serviceResponse;
        }


        public async Task<ServiceResponse> DeleteIngredient(int id)
        {
            ServiceResponse response = new();
            // Ingredient must exist in the first place
            var Ingredient = await _context.Ingredients.FindAsync(id);
            if (Ingredient == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Ingredient cannot be deleted because it does not exist.");
                return response;
            }

            try
            {
                _context.Ingredients.Remove(Ingredient);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error encountered while deleting the Ingredient");
                return response;
            }

            response.Status = ServiceResponse.ServiceStatus.Deleted;

            return response;
            }

            public async Task<IEnumerable<IngredientDto>> ListIngredientsForDessert(int id)
            {

                // join DessertIngredient on Ingredients.Ingredientid = DessertIngredient.Ingredientid WHERE DessertIngredient.dessertid = {id}
                List<Ingredient> Ingredients = await _context.Ingredients
                .Where(i => i.Desserts.Any(d => d.DessertId == id))
                .ToListAsync();

                // empty list of data transfer object IngredientDto
                List<IngredientDto> IngredientDtos = new List<IngredientDto>();
                // foreach Ingredient record in database
                foreach (Ingredient Ingredient in Ingredients)
                {
                // create new instance of IngredientDto, add to list
                    IngredientDtos.Add(new IngredientDto()
                    {
                        IngredientId = Ingredient.IngredientId,
                        IngredientName = Ingredient.IngredientName,
                        IngredientDescription = Ingredient.IngredientDescription
                    });
                }
                // return IngredientDtos
                return IngredientDtos;
            }

        public async Task<ServiceResponse> LinkIngredientToDessert(int ingredientId, int dessertId)
        {
            ServiceResponse serviceResponse = new();

            Ingredient? ingredient = await _context.Ingredients
                .Include(i => i.Desserts)
                .Where(i => i.IngredientId== ingredientId)
                .FirstOrDefaultAsync();
            Dessert? dessert = await _context.Desserts.FindAsync(dessertId);

            // Data must link to a valid entity
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
            try
            {
                ingredient.Desserts.Add(dessert);
                _context.SaveChanges();
            }
            catch (Exception Ex)
            {
                serviceResponse.Messages.Add("There was an issue linking the dessert to the ingredient");
                serviceResponse.Messages.Add(Ex.Message);
            }


            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            return serviceResponse;
        }

        public async Task<ServiceResponse> UnlinkIngredientFromDessert(int ingredientId, int dessertId)
        {
            ServiceResponse serviceResponse = new();

            Ingredient? ingredient = await _context.Ingredients
                .Include(i => i.Desserts)
                .Where(i => i.IngredientId == ingredientId)
                .FirstOrDefaultAsync();
            Dessert? dessert = await _context.Desserts.FindAsync(dessertId);

            // Data must link to a valid entity
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
            try
            {
                ingredient.Desserts.Remove(dessert);
                _context.SaveChanges();
            }
            catch (Exception Ex)
            {
                serviceResponse.Messages.Add("There was an issue unlinking the dessert to the ingredient");
                serviceResponse.Messages.Add(Ex.Message);
            }


            serviceResponse.Status = ServiceResponse.ServiceStatus.Deleted;
            return serviceResponse;
        }
    }
}
