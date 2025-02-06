using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using PassionProject.Data;

namespace PassionProject.Models
{
    public class Dessert
    {
        [Key]
        public int DessertId { get; set; }
        public required string DessertName { get; set; }
        public required string DessertDescription { get; set; }
        public string SpecificTag { get; set; }

        //A dessert can have many ingredients
        public ICollection<DessertIngredient>? Ingredients { get; set; }

        //A dessert has many reviews
        public ICollection<Review>? Reviews { get; set; }

    }
    public class DessertDto
    {
        public int? DessertId { get; set; }
        public string DessertName { get; set; }
        public string DessertDescription { get; set; }
        public string SpecificTag { get; set; }

    }

}
