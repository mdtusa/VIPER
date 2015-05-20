using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VIPER.Models.Entities
{
    public class Process
    {
        public int ProcessID { get; set; }

        [Required(ErrorMessage = "Please enter a Process name")]
        public string Name { get; set; }

        public int Step { get; set; }

        public virtual List<ProcessTime> ProcessTimes { get; set; }

        public virtual List<JobProcess> JobProcesses { get; set; }
    }
}