namespace KyleTanczos.TestKyle.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class CreateStations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Stations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OptionsAsJson = c.String(),
                        OrganizationUnitId = c.Long(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Stations_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AbpOrganizationUnits", t => t.OrganizationUnitId, cascadeDelete: true)
                .Index(t => t.OrganizationUnitId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stations", "OrganizationUnitId", "dbo.AbpOrganizationUnits");
            DropIndex("dbo.Stations", new[] { "OrganizationUnitId" });
            DropTable("dbo.Stations",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Stations_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
