namespace VIPER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableEmployeeArea : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Area",
                c => new
                    {
                        AreaID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.AreaID);
            
            CreateTable(
                "dbo.EmployeeProcess",
                c => new
                    {
                        EmployeeProcessID = c.Int(nullable: false, identity: true),
                        EmployeeID = c.Int(nullable: false),
                        JobProcessID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeProcessID)
                .ForeignKey("dbo.Employee", t => t.EmployeeID, cascadeDelete: true)
                .ForeignKey("dbo.JobProcess", t => t.JobProcessID, cascadeDelete: true)
                .Index(t => t.EmployeeID)
                .Index(t => t.JobProcessID);
            
            CreateTable(
                "dbo.Employee",
                c => new
                    {
                        EmployeeID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        KronosID = c.String(),
                        BadgeNumber = c.String(),
                    })
                .PrimaryKey(t => t.EmployeeID);
            
            AddColumn("dbo.JobProcess", "AreaTypeID", c => c.Int(nullable: false));
            AddColumn("dbo.JobProcess", "Area_AreaID", c => c.Int());
            CreateIndex("dbo.JobProcess", "Area_AreaID");
            AddForeignKey("dbo.JobProcess", "Area_AreaID", "dbo.Area", "AreaID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmployeeProcess", "JobProcessID", "dbo.JobProcess");
            DropForeignKey("dbo.EmployeeProcess", "EmployeeID", "dbo.Employee");
            DropForeignKey("dbo.JobProcess", "Area_AreaID", "dbo.Area");
            DropIndex("dbo.EmployeeProcess", new[] { "JobProcessID" });
            DropIndex("dbo.EmployeeProcess", new[] { "EmployeeID" });
            DropIndex("dbo.JobProcess", new[] { "Area_AreaID" });
            DropColumn("dbo.JobProcess", "Area_AreaID");
            DropColumn("dbo.JobProcess", "AreaTypeID");
            DropTable("dbo.Employee");
            DropTable("dbo.EmployeeProcess");
            DropTable("dbo.Area");
        }
    }
}
