namespace KyleTanczos.TestKyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JayTestMigration : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AbpUsers", "testValue");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AbpUsers", "testValue", c => c.String());
        }
    }
}
