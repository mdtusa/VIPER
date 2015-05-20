using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace VIPER.Models.Entities
{
    public class EmployeeProcess
    {
        public int EmployeeProcessID { get; set; }
        public int EmployeeID { get; set; }
        public int JobProcessID { get; set; }

        [ScriptIgnore]
        public virtual Employee Employee { get; set; }

        [ScriptIgnore]
        public virtual JobProcess JobProcess { get; set; }
    }
}