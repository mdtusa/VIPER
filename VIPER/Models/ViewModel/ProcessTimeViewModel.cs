using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VIPER.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VIPER.Models.ViewModel
{
    public class ProcessTimeViewModel
    {
        public int? RepairTypeID { get; set; }
        public int? SizeID { get; set; }
        public string RepairTypeName { get; set; }
        public string SizeName { get; set; }

        public RepairType RepairType { get; set; }
        public Size Size { get; set; }

        public Decimal? DisassTime { get; set; }
        public int? DisassProcID { get; set; }
        public int? DisassProcTimeID { get; set; }

        public Decimal? CleanTime { get; set; }
        public int? CleanProcID { get; set; }
        public int? CleanProcTimeID { get; set; }

        public Decimal? InspectTime { get; set; }
        public int? InspectProcID { get; set; }
        public int? InspectProcTimeID { get; set; }

        public Decimal? AssembleTime { get; set; }
        public int? AssembleProcID { get; set; }
        public int? AssembleProcTimeID { get; set; }

        public Decimal? AddWorksTime { get; set; }
        public int? AddWorksProcID { get; set; }
        public int? AddWorksProcTimeID { get; set; }

        public Decimal? PaintTime { get; set; }
        public int? PaintProcID { get; set; }
        public int? PaintProcTimeID { get; set; }

        public Decimal? PackagingTime { get; set; }
        public int? PackagingProcID { get; set; }
        public int? PackagingProcTimeID { get; set; }

        public void CreateTypes()
        {
            RepairType repairType = new RepairType();
            repairType.RepairTypeID = this.RepairTypeID.GetValueOrDefault();
            repairType.Name = this.RepairTypeName;
            this.RepairType = repairType;

            Size size = new Size();
            size.SizeID = this.SizeID.GetValueOrDefault();
            size.Name = this.SizeName;
            this.Size = size;
        }
    }
}