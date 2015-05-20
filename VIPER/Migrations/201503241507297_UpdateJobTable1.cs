namespace VIPER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateJobTable1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Job", "SourcedServCost");
            DropColumn("dbo.Job", "PlannedSourcedServCost");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Job", "PlannedSourcedServCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Job", "SourcedServCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
