using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kendo.Mvc.UI;

namespace VIPER.Models.ViewModel
{
    public class ScheduleViewModel : IGanttTask
    {
        public int TaskID { get; set; }
        public int? ParentID { get; set; }
        public int JobID { get; set; }
        public int SchedulePriority { get; set; }

        public string Title { get; set; }

        //private DateTime start;
        public DateTime Start { get; set; }
        

       //private DateTime end;
        public DateTime End { get; set; }
        

        public bool Summary { get; set; }
        public bool Expanded { get; set; }
        public decimal PercentComplete { get; set; }
        public int OrderId { get; set; }
    }
}