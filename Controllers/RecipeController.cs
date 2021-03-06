﻿using HealthyCampusWebApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace HealthyCampusWebApp.Controllers
{
    public class RecipeController : Controller
    {
        //
        // GET: /Recipe/

        const string FilePath = "~/Images/uploads/";
        [Authorize]
        public ActionResult Index()
        {
            var db = new HealthyCampusContext();
            var recipes = db.Recipes;
  
            return View(recipes);
        }

        //
        // GET: /Recipe/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var db = new HealthyCampusContext();
            var recipe = db.Recipes.Find(id);

            return View(recipe);
        }

        //
        // GET: /Recipe/Create
        [Authorize]
        public ActionResult Create()
        {
            Recipe r = new Recipe();

            return View(r);
        }

        //
        // POST: /Recipe/Create

        [HttpPost]
        [Authorize]
        public ActionResult Create(FormCollection values, Recipe recipe)
        {

            string fileNameForDb = "";
            HttpPostedFileBase file = Request.Files["imageFile"];
            
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                string folderPath = Server.MapPath(FilePath);
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
               
                //make unique by adding the date
                string stringPathWithFileName = Server.MapPath( FilePath + 
                                                                Path.GetFileNameWithoutExtension(file.FileName) + 
                                                                DateTime.Now.ToString("ddMMyyyyhhmmss") + 
                                                                Path.GetExtension(file.FileName));

                fileNameForDb = Path.GetFileName(stringPathWithFileName);

                if (System.IO.File.Exists(stringPathWithFileName)) System.IO.File.Delete(stringPathWithFileName);
                    file.SaveAs(stringPathWithFileName);
            }

            string[] keys = values.AllKeys;

            //get all tags
            IEnumerable<string> textKeys = keys.Where(m => m.Contains("tag"));
            IEnumerable<string> listKeys = keys.Where(m => m.Contains("Tags"));
            List<Tag> tags = new List<Tag>();

            foreach(string s in textKeys)
            {
                if((string)values[s] != "")
                    tags.Add(new Tag { TagName = (string)values[s] });
            }

            for (int i = 0; i < listKeys.Count(); i++)
            {
                using( var db = new HealthyCampusContext())
                {
                    tags.Add(db.Tags.Find((string)values[(string)listKeys.ElementAt(i)]));
                }
            }

            //now get all ingredients
            IEnumerable<string> ingredientsKeys = keys.Where(m => m.Contains("ingredientName"));
            IEnumerable<string> amountKeys = keys.Where(m => m.Contains("ingredientAmounts"));
            IEnumerable<string> measurementKeys = keys.Where(m => m.Contains("ingredientAmountMeasure"));
            List<Ingredient> ingredients = new List<Ingredient>();

            for (int i = 0; i < ingredientsKeys.Count(); i++)
			{
                string name = (string)values[(string)ingredientsKeys.ElementAt(i)];
                int grams = 1;
                int.TryParse((string)values[(string)amountKeys.ElementAt(i)], out grams);
                string type = (string)values[(string)measurementKeys.ElementAt(i)];

                ingredients.Add(new Ingredient() {  Name = name,  AmountInGrams = grams, IngredientType = type });
			}

            try
            {
                // TODO: Add insert logic here
                using (var db = new HealthyCampusContext())
                {
                    //recipe.Tags = tags;
                    
                    recipe.Ingredients = ingredients;
                    recipe.ImageURL = fileNameForDb;
                    
                    db.Recipes.Add(recipe);

                    List<string> tagNames = new List<string>();

                    foreach (Tag t in tags)
                    {
                        tagNames.Add(t.TagName);
                    }

     
                    List<string> tagsAlreadyInDb = (from theTags in db.Tags where tagNames.Contains(theTags.TagName) select theTags.TagName).ToList<string>();
                    List<Tag> tagsNotInDB = tags.ToList<Tag>();

                    tagsNotInDB.RemoveAll(t => tagsAlreadyInDb.Contains(t.TagName));

                    foreach(Tag t in tagsNotInDB)
                    {
                        db.Tags.Add(t);
                    }
                    db.SaveChanges();

                    foreach(Tag t in tags)
                    {
                        db.Database.ExecuteSqlCommand("INSERT INTO [dbo].[TagRecipes] ([Tag_TagName], [Recipe_RecipeId]) VALUES ({0},{1});",t.TagName, recipe.RecipeId );
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }   

        //
        // GET: /Recipe/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Recipe/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Recipe/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Recipe/Delete/5

        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //try
            //{
                var db = new HealthyCampusContext();
                var recipe = db.Recipes.Find(id);
                db.Recipes.Remove(recipe);
           

                var allIngredients = db.Database.SqlQuery<Ingredient>("SELECT [IngredientId], [Name], [AmountInGrams], [IngredientType] FROM [Ingredients] WHERE [Recipe_RecipeId] =" + recipe.RecipeId).ToList();
          

                if (allIngredients != null)
                {
                    foreach (Ingredient i in allIngredients)
                    {
                        Ingredient theIng = (from ing in db.Ingredients where ing.IngredientId.Equals(i.IngredientId) select ing).FirstOrDefault();
                       
                        db.Ingredients.Remove(theIng);
                    }
                }
                
                db.SaveChanges();

                return RedirectToAction("Index");
            //}
            //catch
            //{
            //    return View();
            //}
        }
        [HttpGet]
        public ActionResult Image(int id)
        {
            return base.File(FilePath, "image/jpeg");
        }

        public ActionResult RecipeJson()
        {
       
            using(var db = new HealthyCampusContext())
            {
                var allRecipes = from r in db.Recipes select new {r.RecipeId, r.Title, r.Description, r.Method, r.PrepTime, r.CookTime, r.DifficultyLevel, r.ImageURL};

                var jsonSerialiser = new JavaScriptSerializer();
                string json = string.Format("{0}", JsonConvert.SerializeObject(allRecipes));

                return View("Json", null, json);
            }
        }
        public ActionResult TagJson()
        {
            using (var db = new HealthyCampusContext())
            {
                var allTags = from t in db.Tags select new { t.TagName };

                var jsonSerialiser = new JavaScriptSerializer();
                string json = string.Format("{0}", JsonConvert.SerializeObject(allTags));

                return View("Json", null, json);
            }
        }
        public ActionResult IngredientJson()
        {
            using (var db = new HealthyCampusContext())
            {
                //var allIngredients = from i in db.Ingredients
                //                     join r in db.Recipes on i equals i 
                //                     select i.;

                var allIngredients = db.Database.SqlQuery <newIngredient>("SELECT [IngredientId], [Name], [AmountInGrams], [IngredientType], [Recipe_RecipeId] FROM [Ingredients]").ToList();

                var jsonSerialiser = new JavaScriptSerializer();
                string json = string.Format("{0}", JsonConvert.SerializeObject(allIngredients));

                return View("Json", null, json);
            }
        }
        public ActionResult RecipeTagJson()
        {

            using (var db = new HealthyCampusContext())
            {
                var allRecipeTag = db.Database.SqlQuery<RecipeTag>("SELECT TagName = [Tag_TagName] , RecipeId = [Recipe_RecipeId] FROM [TagRecipes]").ToList();

                var jsonSerialiser = new JavaScriptSerializer();
                string json = string.Format("{0}", JsonConvert.SerializeObject(allRecipeTag));

                return View("Json", null, json);
            }
        }
    }

    class RecipeTag{
        public int RecipeId { get; set; }
        public string TagName { get; set; }
    }

    class newIngredient
    {
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public int AmountInGrams { get; set; }
        public string IngredientType { get; set; }
        public int Recipe_RecipeId { get; set; }
    }
}
