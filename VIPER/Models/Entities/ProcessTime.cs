using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VIPER.Models.Entities
{
    public class ProcessTime
    {
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int ProcessTimeID { get; set; }

        public Decimal PlannedTime { get; set; }

        [Index("IX_ProcRepairSize", 1, IsUnique = true)]
        public int ProcessID { get; set; }

        [Index("IX_ProcRepairSize", 2, IsUnique = true)]
        [Required(ErrorMessage = "Please select a Repair Type")]
        public int RepairTypeID { get; set; }

        [Index("IX_ProcRepairSize", 3, IsUnique = true)]
        [Required(ErrorMessage = "Please select a Size")]
        public int SizeID { get; set; }

        public virtual RepairType RepairType { get; set; }

        public virtual Size Size { get; set; }

        public virtual Process Process { get; set; }

        
    }
}