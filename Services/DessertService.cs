using PassionProject.Interfaces;
using PassionProject.Models;
using PassionProject.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace PassionProject.Services
{
    public class DessertService : IDessertService
    {
        private readonly ApplicationDbContext _context;
        // dependency injection of database context
        public DessertService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DessertDto>> ListDesserts()
        {
            // all desserts
            List<Dessert> Desserts = await _context.Desserts
                .ToListAsync();
            // empty list of data transfer object DessertDto
            List<DessertDto> DessertDtos = new List<DessertDto>();
            // foreach Dessert record in database
            foreach (Dessert Dessert in Desserts)
            {
                DessertDtos.Add(new DessertDto()
                {
                    DessertId = Dessert.DessertId,
                    DessertName = Dessert.DessertName,
                    DessertDescription = Dessert.DessertDescription,
                    SpecificTag = Dessert.SpecificTag
                });
            }
            // return DessertDtos
            return DessertDtos;

        }

        public async Task<DessertDto?> FindDessert(int id)
        {
            var Dessert = await _context.Desserts
                .FirstOrDefaultAsync(d => d.DessertId == id);
            if(Dessert == null)
            {
                return null;
            }
            DessertDto DessertDto = new DessertDto()
            {
                DessertId = Dessert.DessertId,
                DessertName = Dessert.DessertName,
                DessertDescription = Dessert.DessertDescription,
                SpecificTag = Dessert.SpecificTag
            };
            return DessertDto;

        }

        public async Task<ServiceResponse> UpdateDessert(DessertDto DessertDto)
        {
            ServiceResponse serviceResponse = new();

            // Create instance of Dessert
            Dessert Dessert = new Dessert()
            {
                DessertId = DessertDto.DessertId,
                DessertName = DessertDto.DessertName,
                DessertDescription = DessertDto.DessertDescription,
                SpecificTag = DessertDto.SpecificTag
            };
            _context.Entry(Dessert).State = EntityState.Modified;

            try
            {
                //SQL Equivalent: Update Desserts set ... where DessertId={id}
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
    
        public async Task<ServiceResponse> AddDessert(DessertDto DessertDto)
        {
            ServiceResponse serviceResponse = new();


            // Create instance of Dessert
            Dessert Dessert = new Dessert()
            {
                DessertName = DessertDto.DessertName,
                DessertDescription = DessertDto.DessertDescription,
                SpecificTag = DessertDto.SpecificTag
            };

            // SQL Equivalent: Insert into Desserts (..) values (..)

            try
            {
                _context.Desserts.Add(Dessert);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("There was an error adding the Dessert.");
                serviceResponse.Messages.Add(ex.Message);

            }


            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = Dessert.DessertId;
            return serviceResponse;
    }


        public async Task<ServiceResponse> DeleteDessert(int id)
        {
            ServiceResponse response = new();
            // Dessert must exist in the first place
            var Dessert = await _context.Desserts.FindAsync(id);
            if (Dessert == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Dessert cannot be deleted because it does not exist.");
                return response;
            }

            try
            {
                _context.Desserts.Remove(Dessert);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error encountered while deleting the dessert");
                return response;
            }

            response.Status = ServiceResponse.ServiceStatus.Deleted;

            return response;

        }

        public async Task<IEnumerable<DessertDto>> ListDessertsForIngredient(int id)
        {
            // join DessertIngredient on dessert.dessertid = DessertIngredient.dessertid WHERE DessertIngredient.ingredientid = {id}
            List<Dessert> Desserts = await _context.Desserts
                .Where(d => d.Ingredients.Any(i => i.IngredientId == id))
                .ToListAsync();

            // empty list of data transfer object DessertDto
            List<DessertDto> DessertDtos = new List<DessertDto>();
            // foreach Dessert record in database
            foreach (Dessert Dessert in Desserts)
            {
                // create new instance of DessertDto, add to list
                DessertDtos.Add(new DessertDto()
                {
                    DessertId = Dessert.DessertId,
                    DessertName = Dessert.DessertName,
                    DessertDescription = Dessert.DessertDescription,
                    SpecificTag = Dessert.SpecificTag
                });
        }
            // return DessertDtos
            return DessertDtos;

    }
        public async Task<ServiceResponse> LinkDessertToIngredient(int dessertId, int ingredientId)
        {
            ServiceResponse serviceResponse = new();

            Dessert? dessert = await _context.Desserts
                .Include(d => d.Ingredients)
                .Where(d => d.DessertId == dessertId)
                .FirstOrDefaultAsync();
            Ingredient? ingredient = await _context.Ingredients.FindAsync(ingredientId);

            // Data must link to a valid entity
            if (ingredient == null || dessert == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                if (ingredient == null)
                {
                    serviceResponse.Messages.Add("Ingredient was not found. ");
                }
                if (dessert == null)
                {
                    serviceResponse.Messages.Add("Dessert was not found.");
                }
                return serviceResponse;
            }
            try
            {
                dessert.Ingredients.Add(ingredient);
                _context.SaveChanges();
            }
            catch (Exception Ex)
            {
                serviceResponse.Messages.Add("There was an issue linking the ingredient to the dessert");
                serviceResponse.Messages.Add(Ex.Message);
            }


            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            return serviceResponse;
        }

        public async Task<ServiceResponse> UnlinkDessertFromIngredient(int dessertId, int ingredientId)
        {
            ServiceResponse serviceResponse = new();

            Dessert? dessert = await _context.Desserts
                .Include(d => d.Ingredients)
                .Where(d => d.DessertId == dessertId)
                .FirstOrDefaultAsync();
            Ingredient? ingredient = await _context.Ingredients.FindAsync(ingredientId);

            // Data must link to a valid entity
            if (ingredient == null || dessert == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                if (ingredient == null)
                {
                    serviceResponse.Messages.Add("Ingredient was not found. ");
                }
                if (dessert == null)
                {
                    serviceResponse.Messages.Add("Dessert was not found.");
                }
                return serviceResponse;
            }
            try
            {
                dessert.Ingredients.Remove(ingredient);
                _context.SaveChanges();
            }
            catch (Exception Ex)
            {
                serviceResponse.Messages.Add("There was an issue unlinking the ingredient to the dessert");
                serviceResponse.Messages.Add(Ex.Message);
            }


            serviceResponse.Status = ServiceResponse.ServiceStatus.Deleted;
            return serviceResponse;
        }

    }
}
