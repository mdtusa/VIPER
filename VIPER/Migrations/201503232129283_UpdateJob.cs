namespace VIPER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateJob : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Job", "OpenDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Job", "ReceivedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Job", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Job", "PromiseDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Job", "CompletionDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Job", "PlannedSparePartsCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Job", "PlannedShippingCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Job", "SourcedServCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Job", "PlannedSourcedServCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Job", "PlannedDutiesCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Job", "ThirdPartyCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Job", "PlannedThirdPartyCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Job", "CreateDate");
            DropColumn("dbo.Job", "DateReceived");
            DropColumn("dbo.Job", "SourcedServicesCost");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Job", "SourcedServicesCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Job", "DateReceived", c => c.DateTime(nullable: false));
            AddColumn("dbo.Job", "CreateDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Job", "PlannedThirdPartyCost");
            DropColumn("dbo.Job", "ThirdPartyCost");
            DropColumn("dbo.Job", "PlannedDutiesCost");
            DropColumn("dbo.Job", "PlannedSourcedServCost");
            DropColumn("dbo.Job", "SourcedServCost");
            DropColumn("dbo.Job", "PlannedShippingCost");
            DropColumn("dbo.Job", "PlannedSparePartsCost");
            DropColumn("dbo.Job", "CompletionDate");
            DropColumn("dbo.Job", "PromiseDate");
            DropColumn("dbo.Job", "StartDate");
            DropColumn("dbo.Job", "ReceivedDate");
            DropColumn("dbo.Job", "OpenDate");
        }
    }
}
