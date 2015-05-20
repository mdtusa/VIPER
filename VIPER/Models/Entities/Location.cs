using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VIPER.Models.Entities
{
    public class Location
    {
        public int LocationID { get; set; }

        [Required(ErrorMessage = "Please enter a Location name")]
        public string Name { get; set; }

        public virtual List<Job> Jobs { get; set; }
    }
}