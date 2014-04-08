using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HealthyCampusWebApp.Models
{

    public class HealthyCampusContext : DbContext
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
    }

    public class Recipe
    {
        public int RecipeId { get; set; }
        [MaxLength(100)]
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Method { get; set; }
        public double PrepTime { get; set; }
        public double CookTime { get; set; }
        public int DifficultyLevel { get; set; }
        public string ImageURL { get; set; }
        
        public virtual List<Tag> Tags { get; set; }
        public virtual List<Ingredient> Ingredients { get; set; }
    } 
    
}