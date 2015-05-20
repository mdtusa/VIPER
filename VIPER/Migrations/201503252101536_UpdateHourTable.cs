namespace VIPER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateHourTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Hour", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Hour", "Name", c => c.String());
        }
    }
}
