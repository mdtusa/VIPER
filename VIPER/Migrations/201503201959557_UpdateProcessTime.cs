namespace VIPER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateProcessTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProcessTime", "PlannedTime", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProcessTime", "PlannedTime", c => c.Int(nullable: false));
        }
    }
}
