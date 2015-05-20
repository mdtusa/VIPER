namespace VIPER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSchedulePriorityToJob : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Job", "SchedulePriority", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Job", "SchedulePriority");
        }
    }
}
