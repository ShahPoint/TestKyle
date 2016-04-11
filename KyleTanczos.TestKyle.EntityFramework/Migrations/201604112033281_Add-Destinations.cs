namespace KyleTanczos.TestKyle.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddDestinations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Hospitals",
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
                    { "DynamicFilter_Hospital_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AbpOrganizationUnits", t => t.OrganizationUnitId, cascadeDelete: true)
                .Index(t => t.OrganizationUnitId);
            
            CreateTable(
                "dbo.OtherFacilities",
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
                    { "DynamicFilter_OtherFacility_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AbpOrganizationUnits", t => t.OrganizationUnitId, cascadeDelete: true)
                .Index(t => t.OrganizationUnitId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OtherFacilities", "OrganizationUnitId", "dbo.AbpOrganizationUnits");
            DropForeignKey("dbo.Hospitals", "OrganizationUnitId", "dbo.AbpOrganizationUnits");
            DropIndex("dbo.OtherFacilities", new[] { "OrganizationUnitId" });
            DropIndex("dbo.Hospitals", new[] { "OrganizationUnitId" });
            DropTable("dbo.OtherFacilities",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_OtherFacility_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Hospitals",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Hospital_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
