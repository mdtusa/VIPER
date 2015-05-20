using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VIPER.Models.Entities
{
    public class Job
    {
        public int JobID { get; set; }

        [Required(ErrorMessage = "Please enter a Vessel name")]
        public string VesselName { get; set; }

        public string JobNumber { get; set; }

        public int Quantity { get; set; }

        public DateTime OpenDate { get; set; }

        public DateTime? ReceivedDate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? PromiseDate { get; set; }

        public DateTime? ShipDate { get; set; }

        public DateTime? CompletionDate { get; set; }

        public decimal SparePartsCost { get; set; }

        public decimal PlannedSparePartsCost { get; set; }

        public decimal ShippingCost { get; set; }

        public decimal PlannedShippingCost { get; set; }

        public decimal DutiesCost { get; set; }

        public decimal PlannedDutiesCost { get; set; }

        public decimal ThirdPartyCost { get; set; }

        public decimal PlannedThirdPartyCost { get; set; }

        public decimal InvoicedTotal { get; set; }

        public int? SchedulePriority { get; set; }

        [NotMapped]
        public decimal EfficiencyRate { get; set; }

        [NotMapped]
        public int TurnTime { get; set; }

        [NotMapped]
        public decimal LaborCost { get; set; }

        [NotMapped]
        public decimal PlannedLaborCost { get; set; }

        [NotMapped]
        public decimal ConsumableCost {get; set;}

        [NotMapped]
        public decimal PackagingCost { get; set; }


        [NotMapped]
        public decimal PlannedConsumable { get; set; }

        [NotMapped]
        public decimal PlannedPackaging { get; set; }

        public decimal? TotalJobCost { get; set; }

        [NotMapped]
        public decimal ActualProfit { get; set; }

        public Boolean? JobComplete { get; set; }

        public Boolean? JobSchedule { get; set; }

        public int? CustomerID { get; set; }

        public int? RepairTypeID { get; set; }

        public int? SizeID { get; set; }

        public int? HourID { get; set; }

        public int? LocationID { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual RepairType RepairType { get; set; }

        public virtual Size Size { get; set; }

        public virtual Hour Hour { get; set; }

        public virtual Location Location { get; set; }

        public virtual List<JobProcess> JobProcesses { get; set; }

    }
}