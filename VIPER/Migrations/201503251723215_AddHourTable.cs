namespace VIPER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHourTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Hour",
                c => new
                    {
                        HourID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.HourID);
            
            AddColumn("dbo.Job", "HourID", c => c.Int(nullable: true));
            CreateIndex("dbo.Job", "HourID");
            AddForeignKey("dbo.Job", "HourID", "dbo.Hour", "HourID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Job", "HourID", "dbo.Hour");
            DropIndex("dbo.Job", new[] { "HourID" });
            DropColumn("dbo.Job", "HourID");
            DropTable("dbo.Hour");
        }
    }
}
