using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HealthyCampusWebApp.Models
{

    public class RecipeContext : DbContext
    {
        public DbSet<Recipe> Recipes { get; set; }
    }

    public class Recipe
    {
        public int RecipeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double PrepTime { get; set; }
        public double CookTime { get; set; }
        public int DifficultyLevel { get; set; }

        public virtual List<Tag> Tags { get; set; }
        public virtual List<Ingredient> Ingredients { get; set; }
    } 
    
}