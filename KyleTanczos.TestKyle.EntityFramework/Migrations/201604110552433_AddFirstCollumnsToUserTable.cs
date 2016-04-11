namespace KyleTanczos.TestKyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFirstCollumnsToUserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AbpUsers", "StateId", c => c.String());
            AddColumn("dbo.AbpUsers", "AgencyCertificationStatus", c => c.String());
            AddColumn("dbo.AbpUsers", "IsEmt", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AbpUsers", "IsEmt");
            DropColumn("dbo.AbpUsers", "AgencyCertificationStatus");
            DropColumn("dbo.AbpUsers", "StateId");
        }
    }
}
