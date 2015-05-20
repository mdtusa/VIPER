using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VIPER.Models.ViewModel
{
    public class EmployeeViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string KronosID { get; set; }
        public string BadgeNumber { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }

    }
}