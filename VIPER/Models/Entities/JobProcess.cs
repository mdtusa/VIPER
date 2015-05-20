using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace VIPER.Models.Entities
{
    public class JobProcess
    {
        public int JobProcessID { get; set; }

        public int JobID { get; set; }

        public int ProcessID { get; set; }

        public decimal PlannedTime { get; set; }

        public decimal ActualTime { get; set; }

        [NotMapped]
        public decimal Difference { get; set; }

        public int ScheduleWeek { get; set; }

        public int Status { get; set; }

        public decimal PercentComplete { get; set; }

        public DateTime Start {get; set;}

        public DateTime End { get; set; }
        
        [ScriptIgnore]
        public virtual Job Job { get; set; }

        [ScriptIgnore]
        public virtual Process Process { get; set; }

        public virtual List<EmployeeProcess> EmployeeProcesses { get; set; }

        public int AreaTypeID { get; set; }

        public virtual Area Area { get; set; }
    }
}