namespace VIPER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobTableColumnDrop : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Job", "ReceivedDate");
            DropColumn("dbo.Job", "StartDate");
            DropColumn("dbo.Job", "PromiseDate");
            DropColumn("dbo.Job", "ShipDate");
            DropColumn("dbo.Job", "CompletionDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Job", "CompletionDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Job", "ShipDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Job", "PromiseDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Job", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Job", "ReceivedDate", c => c.DateTime(nullable: false));
        }
    }
}
