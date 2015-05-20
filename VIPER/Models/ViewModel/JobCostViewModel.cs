using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VIPER.Models.ViewModel
{
    public class JobCostViewModel
    {
        public int PivotJobID { get; set; }

        public int JobID { get; set; }

        public string CostType { get; set; }

        public decimal Planned { get; set; }

        public decimal Actual { get; set; }

        public decimal Difference { get; set; }
    }
}