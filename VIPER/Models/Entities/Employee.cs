using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VIPER.Models.Entities
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string KronosID { get; set; }
        public string BadgeNumber { get; set; }
        public string Type { get; set; }

        public virtual List<EmployeeProcess> EmployeeProcesses { get; set; }
    }
}