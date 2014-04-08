using HealthyCampusWebApp.Models;
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

        public ActionResult Index()
        {
            var db = new HealthyCampusContext();
            var recipes = db.Recipes;
  
            return View(recipes);
        }

        //
        // GET: /Recipe/Details/5

        public ActionResult Details(int id)
        {
            var db = new HealthyCampusContext();
            var recipe = db.Recipes.Find(id);

            return View(recipe);
        }

        //
        // GET: /Recipe/Create

        public ActionResult Create()
        {
            Recipe r = new Recipe();

            return View(r);
        }

        //
        // POST: /Recipe/Create

        [HttpPost]
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

            foreach(string s in listKeys)
            {
                tags.Add(new HealthyCampusContext().Tags.Find(s));
            }

            //now get all ingredients
            IEnumerable<string> ingredientsKeys = keys.Where(m => m.Contains("ingredientName"));
            IEnumerable<string> amountKeys = keys.Where(m => m.Contains("ingredientAmount"));
            IEnumerable<string> measurementKeys = keys.Where(m => m.Contains("ingredientAmountMeasure"));
            List<Ingredient> ingredients = new List<Ingredient>();

            for (int i = 0; i < ingredientsKeys.Count(); i++)
			{
                string name = (string)values[(string)ingredientsKeys.ElementAt(0)];
                int grams = int.Parse((string)values[(string)amountKeys.ElementAt(0)]);
                string type = (string)values[(string)measurementKeys.ElementAt(0)];

                ingredients.Add(new Ingredient() {  Name = name,  AmountInGrams = grams, IngredientType = type });
			}
           
            try
            {
                // TODO: Add insert logic here
                using (var db = new HealthyCampusContext())
                {
                    recipe.Tags = tags;
                    recipe.Ingredients = ingredients;
                    recipe.ImageURL = fileNameForDb;
                    
                    db.Recipes.Add(recipe);
                    db.SaveChanges();
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

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Recipe/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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
                var allIngredients = from i in db.Ingredients select i;

                var jsonSerialiser = new JavaScriptSerializer();
                string json = string.Format("{0}", JsonConvert.SerializeObject(allIngredients));

                return View("Json", null, json);
            }
        }
        public ActionResult RecipeTagJson()
        {

            using (var db = new HealthyCampusContext())
            {

                var allRecipes = from r in db.Recipes select r;
                var allTags = from t in db.Tags select t;

                List<RecipeTag> rt = new List<RecipeTag>();

                foreach (Recipe r in allRecipes)
                {
                    foreach(Tag t in r.Tags)
                    {
                        rt.Add(new RecipeTag() { RecipeId = r.RecipeId, TagName = t.TagName });
                    }
                }

                var jsonSerialiser = new JavaScriptSerializer();
                string json = string.Format("{0}", JsonConvert.SerializeObject(rt));

                return View("Json", null, json);
            }
        }
    }

    class RecipeTag{
        public int RecipeId { get; set; }
        public string TagName { get; set; }
    }
}
