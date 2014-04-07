using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthyCampusWebApp.Models
{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public int AmountInGrams { get; set; }
        public string IngredientType { get; set; }
    }
}