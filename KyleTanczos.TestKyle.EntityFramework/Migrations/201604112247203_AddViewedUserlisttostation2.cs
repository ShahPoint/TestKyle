namespace KyleTanczos.TestKyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddViewedUserlisttostation2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AbpUsers", "Stations_Id", c => c.Int());
            CreateIndex("dbo.AbpUsers", "Stations_Id");
            AddForeignKey("dbo.AbpUsers", "Stations_Id", "dbo.Stations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AbpUsers", "Stations_Id", "dbo.Stations");
            DropIndex("dbo.AbpUsers", new[] { "Stations_Id" });
            DropColumn("dbo.AbpUsers", "Stations_Id");
        }

        
    }
}
