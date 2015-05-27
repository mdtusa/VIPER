namespace VIPER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PackagingInJob : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Job", "PackagingCost", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Job", "PlannedPackaging", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Job", "PlannedPackaging");
            DropColumn("dbo.Job", "PackagingCost");
        }
    }
}
