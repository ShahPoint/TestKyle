namespace KyleTanczos.TestKyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeAddViewedUserlisttostation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AbpUsers", "Stations_Id", "dbo.Stations");
            DropIndex("dbo.AbpUsers", new[] { "Stations_Id" });
            DropColumn("dbo.AbpUsers", "Stations_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AbpUsers", "Stations_Id", c => c.Int());
            CreateIndex("dbo.AbpUsers", "Stations_Id");
            AddForeignKey("dbo.AbpUsers", "Stations_Id", "dbo.Stations", "Id");
        }
    }
}
