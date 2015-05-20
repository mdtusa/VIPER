namespace VIPER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropSchedulePriority : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Job", "SchedulePriority");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Job", "SchedulePriority", c => c.Int());
        }
    }
}
