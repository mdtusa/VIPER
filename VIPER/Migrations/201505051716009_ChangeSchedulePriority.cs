namespace VIPER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeSchedulePriority : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Job", "SchedulePriority", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Job", "SchedulePriority", c => c.Int(nullable: false));
        }
    }
}
