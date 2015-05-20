namespace VIPER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTotalJobCostColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Job", "TotalJobCost", c => c.Decimal(nullable: true, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Job", "TotalJobCost");
        }
    }
}
