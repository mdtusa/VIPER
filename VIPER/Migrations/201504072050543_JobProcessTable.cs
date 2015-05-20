namespace VIPER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobProcessTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobProcess", "PercentComplete", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.JobProcess", "Start", c => c.DateTime(nullable: false));
            AddColumn("dbo.JobProcess", "End", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobProcess", "End");
            DropColumn("dbo.JobProcess", "Start");
            DropColumn("dbo.JobProcess", "PercentComplete");
        }
    }
}
