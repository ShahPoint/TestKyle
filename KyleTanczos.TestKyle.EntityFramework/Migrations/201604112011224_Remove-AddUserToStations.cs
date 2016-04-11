namespace KyleTanczos.TestKyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveAddUserToStations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Stations", "CreatorUserId", "dbo.AbpUsers");
            DropIndex("dbo.Stations", new[] { "CreatorUserId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Stations", "CreatorUserId");
            AddForeignKey("dbo.Stations", "CreatorUserId", "dbo.AbpUsers", "Id");
        }
    }
}
