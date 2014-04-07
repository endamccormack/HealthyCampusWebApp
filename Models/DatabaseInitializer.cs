using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HealthyCampusWebApp.Models
{
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<HealthyCampusContext>
    {
        protected override void Seed(HealthyCampusContext context)
        {
            // Seed code here
            using (var db = new HealthyCampusContext())
            {
                #region
                Tag t1 = new Tag() { TagName = "Breakfast" };
                Tag t2 = new Tag() { TagName = "Snack" };

                List<Tag> tags = new List<Tag>();
                tags.Add(t1);
                tags.Add(t2);
                #endregion
                #region
                Ingredient i1 = new Ingredient()
                {
                    Name = "Flour",
                    AmountInGrams = 400,
                    IngredientType = "grams"
                };

                Ingredient i2 = new Ingredient()
                {
                    Name = "Eg",
                    AmountInGrams = 400
                };
                Ingredient i3 = new Ingredient()
                {
                    Name = "Milk",
                    AmountInGrams = 400,
                    IngredientType = "mililiters"
                };

                List<Ingredient> ingredients = new List<Ingredient>();
                ingredients.Add(i1);
                ingredients.Add(i2);
                ingredients.Add(i3);
                #endregion

                Recipe r = new Recipe()
                {
                    Title = "Pancakes",
                    Description = "Lorem Ipsum blahhhhhhhhhhhhhhhhhhhhh",
                    PrepTime = 1.5,
                    CookTime = 10,
                    DifficultyLevel = 5,
                    Tags = tags,
                    Ingredients = ingredients
                };
                db.Recipes.Add(r);
                db.SaveChanges();
            }
        }
    }
}