using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VIPER.Models.Entities;

namespace VIPER.Models.ViewModel
{
    public class JobViewModel
    {
        public int JobID { get; set; }

        [Required(ErrorMessage = "Vessel Name is required")]
        public string VesselName { get; set; }

        [Required(ErrorMessage = "Job Number is required")]
        public string JobNumber { get; set; }

        public int Quantity { get; set; }

        public DateTime OpenDate { get; set; }

        public DateTime DateReceived { get; set; }

        public DateTime ShipDate { get; set; }

        public decimal SparePartsCost { get; set; }

        public decimal ShippingCost { get; set; }

        public decimal ThirdPartyCost { get; set; }

        public decimal DutiesCost { get; set; }

        public decimal InvoicedTotal { get; set; }

        public decimal EfficiencyRate { get; set; }

        public int TurnTime { get; set; }

        public decimal LaborCost { get; set; }

        public decimal ConsumAndPackCost { get; set; }

        public decimal TotalJobCost { get; set; }

        public decimal ActualProfit { get; set; }

        public int RepairTypeID { get; set; }

        public int SizeID { get; set; }

        public int HourID { get; set; }

        public RepairTypeViewModel RepairType { get; set; }

        public SizeViewModel Size { get; set; }

        public HourViewModel Hour { get; set; }

    }
}