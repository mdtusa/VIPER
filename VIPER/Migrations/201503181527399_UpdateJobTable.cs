namespace VIPER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateJobTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Job", "JobComplete", c => c.Boolean(nullable: false));
            AddColumn("dbo.Job", "JobSchedule", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Job", "JobSchedule");
            DropColumn("dbo.Job", "JobComplete");
        }
    }
}
