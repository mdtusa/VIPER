namespace VIPER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyEmployeeTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employee", "Type", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employee", "Type");
        }
    }
}
