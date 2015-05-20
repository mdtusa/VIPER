namespace VIPER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobTableColumnAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Job", "ReceivedDate", c => c.DateTime());
            AddColumn("dbo.Job", "StartDate", c => c.DateTime());
            AddColumn("dbo.Job", "PromiseDate", c => c.DateTime());
            AddColumn("dbo.Job", "ShipDate", c => c.DateTime());
            AddColumn("dbo.Job", "CompletionDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Job", "CompletionDate");
            DropColumn("dbo.Job", "ShipDate");
            DropColumn("dbo.Job", "PromiseDate");
            DropColumn("dbo.Job", "StartDate");
            DropColumn("dbo.Job", "ReceivedDate");
        }
    }
}
