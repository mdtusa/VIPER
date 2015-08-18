using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;


namespace VIPER.Models.Entities
{
    public class Customer
    {
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "Please enter a Customer name")]
        public string Name { get; set; }

        [ScriptIgnore]
        public virtual List<AllJob> AllJobs { get; set; }

    }
}