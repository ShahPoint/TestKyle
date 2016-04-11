namespace KyleTanczos.TestKyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserToStations : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Stations", "CreatorUserId");
            AddForeignKey("dbo.Stations", "CreatorUserId", "dbo.AbpUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stations", "CreatorUserId", "dbo.AbpUsers");
            DropIndex("dbo.Stations", new[] { "CreatorUserId" });
        }
    }
}
