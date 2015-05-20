namespace VIPER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Holiday : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Holiday",
                c => new
                    {
                        HolidayID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.HolidayID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Holiday");
        }
    }
}
