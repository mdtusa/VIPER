using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VIPER.Models.ViewModel
{
    public class JobProcessViewModel
    {
        public int JobProcessID { get; set; }

        public int JobID { get; set; }

        public int ProcessID { get; set; }

        public decimal PlannedTime { get; set; }

        public decimal ActualTime { get; set; }

        public decimal Difference { get; set; }

        public int ScheduleWeek { get; set; }

        public int Status { get; set; }

        public decimal PercentComplete { get; set; }

        public DateTime Start {get; set;}

        public DateTime End { get; set; }
       
        public string ProcessName { get; set; }

        public string ImageURL { get; set; }

        public string Note { get; set; }

        public decimal ReworkTime { get; set; }
    }
}