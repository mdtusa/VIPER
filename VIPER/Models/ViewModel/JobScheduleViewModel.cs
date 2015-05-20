using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VIPER.Models.ViewModel
{
    public class JobScheduleViewModel
    {
        public int JobID { get; set; }

        public DateTime? ReceivedDate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? PromiseDate { get; set; }

        public DateTime? ShipDate { get; set; }

        public DateTime? CompletionDate { get; set; }

        public int TurnTime { get; set; }
    }
}