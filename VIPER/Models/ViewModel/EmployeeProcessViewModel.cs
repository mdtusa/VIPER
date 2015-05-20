using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VIPER.Models.ViewModel
{
    public class EmployeeProcessViewModel
    {
        public int EmployeeProcessID { get; set; }
        public int EmployeeID { get; set; }
        public int JobProcessID { get; set; }
        public decimal Units { get; set; } 
    }
}