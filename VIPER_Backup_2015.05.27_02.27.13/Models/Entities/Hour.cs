using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VIPER.Models.Entities
{
    public class Hour
    {
        public int HourID { get; set; }

        [Required(ErrorMessage = "Please enter a Hour name")]
        public string Name { get; set; }
    }
}