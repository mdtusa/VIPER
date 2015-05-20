using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VIPER.Models.Entities
{
    public class RepairType
    {
        public int RepairTypeID { get; set; }

        [Required(ErrorMessage = "Please enter a Repair Type name")]
        public string Name { get; set; }

        //public virtual List<Size> Sizes { get; set; }

        //public virtual List<ProcessTime> ProcessTimes { get; set; }

        //public virtual List<Job> Jobs { get; set; }
    }
}