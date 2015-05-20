namespace VIPER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnToJob : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JobProcess", "ProcessTimeID", "dbo.ProcessTime");
            DropIndex("dbo.JobProcess", new[] { "ProcessTimeID" });
            RenameColumn(table: "dbo.JobProcess", name: "ProcessTimeID", newName: "ProcessTime_ProcessTimeID");
            AddColumn("dbo.Job", "JobNumber", c => c.String());
            AddColumn("dbo.Job", "SparePartsCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Job", "ShippingCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Job", "SourcedServicesCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Job", "DutiesCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Job", "InvoicedTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.JobProcess", "ProcessID", c => c.Int(nullable: false));
            AlterColumn("dbo.JobProcess", "ProcessTime_ProcessTimeID", c => c.Int());
            AlterColumn("dbo.JobProcess", "PlannedTime", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.JobProcess", "ActualTime", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            CreateIndex("dbo.JobProcess", "ProcessID");
            CreateIndex("dbo.JobProcess", "ProcessTime_ProcessTimeID");
            AddForeignKey("dbo.JobProcess", "ProcessID", "dbo.Process", "ProcessID", cascadeDelete: false);
            AddForeignKey("dbo.JobProcess", "ProcessTime_ProcessTimeID", "dbo.ProcessTime", "ProcessTimeID", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobProcess", "ProcessTime_ProcessTimeID", "dbo.ProcessTime");
            DropForeignKey("dbo.JobProcess", "ProcessID", "dbo.Process");
            DropIndex("dbo.JobProcess", new[] { "ProcessTime_ProcessTimeID" });
            DropIndex("dbo.JobProcess", new[] { "ProcessID" });
            AlterColumn("dbo.JobProcess", "ActualTime", c => c.Int(nullable: false));
            AlterColumn("dbo.JobProcess", "PlannedTime", c => c.Int(nullable: false));
            AlterColumn("dbo.JobProcess", "ProcessTime_ProcessTimeID", c => c.Int(nullable: false));
            DropColumn("dbo.JobProcess", "ProcessID");
            DropColumn("dbo.Job", "InvoicedTotal");
            DropColumn("dbo.Job", "DutiesCost");
            DropColumn("dbo.Job", "SourcedServicesCost");
            DropColumn("dbo.Job", "ShippingCost");
            DropColumn("dbo.Job", "SparePartsCost");
            DropColumn("dbo.Job", "JobNumber");
            RenameColumn(table: "dbo.JobProcess", name: "ProcessTime_ProcessTimeID", newName: "ProcessTimeID");
            CreateIndex("dbo.JobProcess", "ProcessTimeID");
            AddForeignKey("dbo.JobProcess", "ProcessTimeID", "dbo.ProcessTime", "ProcessTimeID", cascadeDelete: true);
        }
    }
}
