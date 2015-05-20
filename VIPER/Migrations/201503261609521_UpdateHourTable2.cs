namespace VIPER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateHourTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Hour", "Name", c => c.String(nullable: false));
            DropColumn("dbo.Hour", "HourName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Hour", "HourName", c => c.String(nullable: false));
            DropColumn("dbo.Hour", "Name");
        }
    }
}
