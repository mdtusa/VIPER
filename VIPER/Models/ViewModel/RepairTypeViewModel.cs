using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VIPER.Models.ViewModel
{
    public class RepairTypeViewModel
    {
        public int RepairTypeID { get; set; }

        [Required(ErrorMessage = "Repair Type is required")]
        public string Name { get; set; }
    }
}