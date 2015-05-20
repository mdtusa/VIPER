using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VIPER.Models.Entities
{
    public class Holiday
    {
        public int HolidayID { get; set; }

        [Required(ErrorMessage = "Please enter a Holiday name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a Holiday date")]
        public DateTime Date { get; set; }
    }
}