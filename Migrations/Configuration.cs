namespace HealthyCampusWebApp.Migrations
{
    using HealthyCampusWebApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<HealthyCampusWebApp.Models.HealthyCampusContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
        protected override void Seed(HealthyCampusWebApp.Models.HealthyCampusContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            try { 
                #region
                Tag t1 = new Tag() { TagName = "breakfast" };
                Tag t2 = new Tag() { TagName = "snack" };

                List<Tag> tags = new List<Tag>();
                tags.Add(t1);
                tags.Add(t2);
                #endregion
                #region
                Ingredient i1 = new Ingredient()
                {
                    Name = "flour",
                    AmountInGrams = 400,
                    IngredientType = "grams"
                };

                Ingredient i2 = new Ingredient()
                {
                    Name = "eg",
                    AmountInGrams = 400
                };
                Ingredient i3 = new Ingredient()
                {
                    Name = "milk",
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
                    Title = "pancakes",
                    Description = "lorem ipsum blahhhhhhhhhhhhhhhhhhhhh",
                    PrepTime = 1.5,
                    CookTime = 10,
                    DifficultyLevel = 5,
                    Tags = tags,
                    Ingredients = ingredients
                };
                context.Recipes.AddOrUpdate(r);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }
        }
    }
}
