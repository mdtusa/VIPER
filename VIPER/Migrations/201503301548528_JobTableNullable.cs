namespace VIPER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobTableNullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Job", "CustomerID", "dbo.Customer");
            DropForeignKey("dbo.Job", "HourID", "dbo.Hour");
            DropForeignKey("dbo.Job", "LocationID", "dbo.Location");
            DropForeignKey("dbo.Job", "RepairTypeID", "dbo.RepairType");
            DropForeignKey("dbo.Job", "SizeID", "dbo.Size");
            DropIndex("dbo.Job", new[] { "CustomerID" });
            DropIndex("dbo.Job", new[] { "RepairTypeID" });
            DropIndex("dbo.Job", new[] { "SizeID" });
            DropIndex("dbo.Job", new[] { "HourID" });
            DropIndex("dbo.Job", new[] { "LocationID" });
            AlterColumn("dbo.Job", "JobComplete", c => c.Boolean());
            AlterColumn("dbo.Job", "JobSchedule", c => c.Boolean());
            AlterColumn("dbo.Job", "CustomerID", c => c.Int());
            AlterColumn("dbo.Job", "RepairTypeID", c => c.Int());
            AlterColumn("dbo.Job", "SizeID", c => c.Int());
            AlterColumn("dbo.Job", "HourID", c => c.Int());
            AlterColumn("dbo.Job", "LocationID", c => c.Int());
            CreateIndex("dbo.Job", "CustomerID");
            CreateIndex("dbo.Job", "RepairTypeID");
            CreateIndex("dbo.Job", "SizeID");
            CreateIndex("dbo.Job", "HourID");
            CreateIndex("dbo.Job", "LocationID");
            AddForeignKey("dbo.Job", "CustomerID", "dbo.Customer", "CustomerID");
            AddForeignKey("dbo.Job", "HourID", "dbo.Hour", "HourID");
            AddForeignKey("dbo.Job", "LocationID", "dbo.Location", "LocationID");
            AddForeignKey("dbo.Job", "RepairTypeID", "dbo.RepairType", "RepairTypeID");
            AddForeignKey("dbo.Job", "SizeID", "dbo.Size", "SizeID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Job", "SizeID", "dbo.Size");
            DropForeignKey("dbo.Job", "RepairTypeID", "dbo.RepairType");
            DropForeignKey("dbo.Job", "LocationID", "dbo.Location");
            DropForeignKey("dbo.Job", "HourID", "dbo.Hour");
            DropForeignKey("dbo.Job", "CustomerID", "dbo.Customer");
            DropIndex("dbo.Job", new[] { "LocationID" });
            DropIndex("dbo.Job", new[] { "HourID" });
            DropIndex("dbo.Job", new[] { "SizeID" });
            DropIndex("dbo.Job", new[] { "RepairTypeID" });
            DropIndex("dbo.Job", new[] { "CustomerID" });
            AlterColumn("dbo.Job", "LocationID", c => c.Int(nullable: false));
            AlterColumn("dbo.Job", "HourID", c => c.Int(nullable: false));
            AlterColumn("dbo.Job", "SizeID", c => c.Int(nullable: false));
            AlterColumn("dbo.Job", "RepairTypeID", c => c.Int(nullable: false));
            AlterColumn("dbo.Job", "CustomerID", c => c.Int(nullable: false));
            AlterColumn("dbo.Job", "JobSchedule", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Job", "JobComplete", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Job", "LocationID");
            CreateIndex("dbo.Job", "HourID");
            CreateIndex("dbo.Job", "SizeID");
            CreateIndex("dbo.Job", "RepairTypeID");
            CreateIndex("dbo.Job", "CustomerID");
            AddForeignKey("dbo.Job", "SizeID", "dbo.Size", "SizeID", cascadeDelete: true);
            AddForeignKey("dbo.Job", "RepairTypeID", "dbo.RepairType", "RepairTypeID", cascadeDelete: true);
            AddForeignKey("dbo.Job", "LocationID", "dbo.Location", "LocationID", cascadeDelete: true);
            AddForeignKey("dbo.Job", "HourID", "dbo.Hour", "HourID", cascadeDelete: true);
            AddForeignKey("dbo.Job", "CustomerID", "dbo.Customer", "CustomerID", cascadeDelete: true);
        }
    }
}
