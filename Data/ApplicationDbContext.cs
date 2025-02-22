﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PassionProject.Models;

namespace PassionProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Dessert> Desserts { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Instruction> Instructions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //public DbSet<PassionProject.Models.Instruction> Insructions { get; set; } = default!;
    }
}
