using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;

namespace VIPER.Models.Entities
{
    public class RepairType
    {
        public int RepairTypeID { get; set; }

        [Required(ErrorMessage = "Please enter a Repair Type name")]
        public string Name { get; set; }

        [ScriptIgnore]
        public virtual List<Size> Sizes { get; set; }

        [ScriptIgnore]
        public virtual List<ProcessTime> ProcessTimes { get; set; }

        [ScriptIgnore]
        public virtual List<Job> Jobs { get; set; }
    }
}