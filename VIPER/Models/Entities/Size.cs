using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace VIPER.Models.Entities
{
    public class Size
    {
        public int SizeID { get; set; }

        [Required(ErrorMessage = "Please enter a Size name")]
        public string Name { get; set; }

        public int RepairTypeID { get; set; }

        public virtual RepairType RepairType { get; set; }

        [ScriptIgnore]
        public virtual List<Job> Jobs { get; set; }

    }
}