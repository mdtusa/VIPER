using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VIPER.Models.ViewModel
{
    public class SizeViewModel
    {
        public int SizeID { get; set; }

        [Required(ErrorMessage = "Size is required")]
        public string Name { get; set; }

        public int RepairTypeID { get; set; }

        public RepairTypeViewModel RepairType { get; set; }
    }
}