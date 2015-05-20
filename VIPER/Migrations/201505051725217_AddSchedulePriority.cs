namespace VIPER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSchedulePriority : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Job", "SchedulePriority", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Job", "SchedulePriority");
        }
    }
}
