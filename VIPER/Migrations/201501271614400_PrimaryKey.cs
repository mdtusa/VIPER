namespace VIPER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrimaryKey : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        CustomerID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CustomerID);
            
            CreateTable(
                "dbo.Job",
                c => new
                    {
                        JobID = c.Int(nullable: false, identity: true),
                        VesselName = c.String(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        DateReceived = c.DateTime(nullable: false),
                        ShipDate = c.DateTime(nullable: false),
                        Quantity = c.Int(nullable: false),
                        CustomerID = c.Int(nullable: false),
                        RepairTypeID = c.Int(nullable: false),
                        SizeID = c.Int(nullable: false),
                        LocationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JobID)
                .ForeignKey("dbo.Customer", t => t.CustomerID, cascadeDelete: true)
                .ForeignKey("dbo.Size", t => t.SizeID, cascadeDelete: true)
                .ForeignKey("dbo.Location", t => t.LocationID, cascadeDelete: true)
                .ForeignKey("dbo.RepairType", t => t.RepairTypeID, cascadeDelete: true)
                .Index(t => t.CustomerID)
                .Index(t => t.RepairTypeID)
                .Index(t => t.SizeID)
                .Index(t => t.LocationID);
            
            CreateTable(
                "dbo.JobProcess",
                c => new
                    {
                        JobProcessID = c.Int(nullable: false, identity: true),
                        JobID = c.Int(nullable: false),
                        ProcessTimeID = c.Int(nullable: false),
                        PlannedTime = c.Int(nullable: false),
                        ActualTime = c.Int(nullable: false),
                        ScheduleWeek = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        ProcessTime_ProcessTimeID = c.Int(),
                        ProcessTime_ProcessID = c.Int(),
                        ProcessTime_RepairTypeID = c.Int(),
                        ProcessTime_SizeID = c.Int(),
                    })
                .PrimaryKey(t => t.JobProcessID)
                .ForeignKey("dbo.Job", t => t.JobID, cascadeDelete: true)
                .ForeignKey("dbo.ProcessTime", t => new { t.ProcessTime_ProcessTimeID, t.ProcessTime_ProcessID, t.ProcessTime_RepairTypeID, t.ProcessTime_SizeID })
                .Index(t => t.JobID)
                .Index(t => new { t.ProcessTime_ProcessTimeID, t.ProcessTime_ProcessID, t.ProcessTime_RepairTypeID, t.ProcessTime_SizeID });
            
            CreateTable(
                "dbo.ProcessTime",
                c => new
                    {
                        ProcessTimeID = c.Int(nullable: false),
                        ProcessID = c.Int(nullable: false),
                        RepairTypeID = c.Int(nullable: false),
                        SizeID = c.Int(nullable: false),
                        PlannedTime = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProcessTimeID, t.ProcessID, t.RepairTypeID, t.SizeID })
                .ForeignKey("dbo.Process", t => t.ProcessID, cascadeDelete: true)
                .ForeignKey("dbo.RepairType", t => t.RepairTypeID, cascadeDelete: true)
                .ForeignKey("dbo.Size", t => t.SizeID, cascadeDelete: true)
                .Index(t => t.ProcessID)
                .Index(t => t.RepairTypeID)
                .Index(t => t.SizeID);
            
            CreateTable(
                "dbo.Process",
                c => new
                    {
                        ProcessID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Step = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProcessID);
            
            CreateTable(
                "dbo.RepairType",
                c => new
                    {
                        RepairTypeID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.RepairTypeID);
            
            CreateTable(
                "dbo.Size",
                c => new
                    {
                        SizeID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        RepairTypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SizeID)
                .ForeignKey("dbo.RepairType", t => t.RepairTypeID, cascadeDelete: false)
                .Index(t => t.RepairTypeID);
            
            CreateTable(
                "dbo.Location",
                c => new
                    {
                        LocationID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.LocationID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Job", "RepairTypeID", "dbo.RepairType");
            DropForeignKey("dbo.Job", "LocationID", "dbo.Location");
            DropForeignKey("dbo.ProcessTime", "SizeID", "dbo.Size");
            DropForeignKey("dbo.Size", "RepairTypeID", "dbo.RepairType");
            DropForeignKey("dbo.Job", "SizeID", "dbo.Size");
            DropForeignKey("dbo.ProcessTime", "RepairTypeID", "dbo.RepairType");
            DropForeignKey("dbo.ProcessTime", "ProcessID", "dbo.Process");
            DropForeignKey("dbo.JobProcess", new[] { "ProcessTime_ProcessTimeID", "ProcessTime_ProcessID", "ProcessTime_RepairTypeID", "ProcessTime_SizeID" }, "dbo.ProcessTime");
            DropForeignKey("dbo.JobProcess", "JobID", "dbo.Job");
            DropForeignKey("dbo.Job", "CustomerID", "dbo.Customer");
            DropIndex("dbo.Size", new[] { "RepairTypeID" });
            DropIndex("dbo.ProcessTime", new[] { "SizeID" });
            DropIndex("dbo.ProcessTime", new[] { "RepairTypeID" });
            DropIndex("dbo.ProcessTime", new[] { "ProcessID" });
            DropIndex("dbo.JobProcess", new[] { "ProcessTime_ProcessTimeID", "ProcessTime_ProcessID", "ProcessTime_RepairTypeID", "ProcessTime_SizeID" });
            DropIndex("dbo.JobProcess", new[] { "JobID" });
            DropIndex("dbo.Job", new[] { "LocationID" });
            DropIndex("dbo.Job", new[] { "SizeID" });
            DropIndex("dbo.Job", new[] { "RepairTypeID" });
            DropIndex("dbo.Job", new[] { "CustomerID" });
            DropTable("dbo.Location");
            DropTable("dbo.Size");
            DropTable("dbo.RepairType");
            DropTable("dbo.Process");
            DropTable("dbo.ProcessTime");
            DropTable("dbo.JobProcess");
            DropTable("dbo.Job");
            DropTable("dbo.Customer");
        }
    }
}
