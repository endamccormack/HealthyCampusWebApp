using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HealthyCampusWebApp.Models
{
    
    public class Tag
    {
        [MaxLength(40)]
        [Key]
        public string TagName { get; set; }

        public virtual List<Recipe> Recipes { get; set; }
    }
}