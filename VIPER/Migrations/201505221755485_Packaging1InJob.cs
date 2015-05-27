namespace VIPER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Packaging1InJob : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Job", "InvoicedTotal", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Job", "InvoicedTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
