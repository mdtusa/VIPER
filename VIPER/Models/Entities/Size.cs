using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VIPER.Models.Entities
{
    public class Size
    {
        public int SizeID { get; set; }

        [Required(ErrorMessage = "Please enter a Size name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please select Repair Type")]
        public int RepairTypeID { get; set; }

        public virtual RepairType RepairType { get; set; }

        public virtual List<Job> Jobs { get; set; }

    }
}