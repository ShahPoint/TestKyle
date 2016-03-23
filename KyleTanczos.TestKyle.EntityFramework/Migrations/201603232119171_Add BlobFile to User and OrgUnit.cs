namespace KyleTanczos.TestKyle.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddBlobFiletoUserandOrgUnit : DbMigration
    {
        public override void Up()
        {
            AlterTableAnnotations(
                "dbo.AbpOrganizationUnits",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        ParentId = c.Long(),
                        Code = c.String(nullable: false, maxLength: 128),
                        DisplayName = c.String(nullable: false, maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_Organization_MayHaveTenant",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                    { 
                        "DynamicFilter_Organization_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
            AddColumn("dbo.blobFiles", "UserId", c => c.Long(nullable: false));
            AddColumn("dbo.blobFiles", "OrganizationUnitId", c => c.Long(nullable: false));
            AddColumn("dbo.AbpOrganizationUnits", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.blobFiles", "UserId");
            CreateIndex("dbo.blobFiles", "OrganizationUnitId");
            AddForeignKey("dbo.blobFiles", "OrganizationUnitId", "dbo.AbpOrganizationUnits", "Id", cascadeDelete: true);
            AddForeignKey("dbo.blobFiles", "UserId", "dbo.AbpUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.blobFiles", "UserId", "dbo.AbpUsers");
            DropForeignKey("dbo.blobFiles", "OrganizationUnitId", "dbo.AbpOrganizationUnits");
            DropIndex("dbo.blobFiles", new[] { "OrganizationUnitId" });
            DropIndex("dbo.blobFiles", new[] { "UserId" });
            DropColumn("dbo.AbpOrganizationUnits", "Discriminator");
            DropColumn("dbo.blobFiles", "OrganizationUnitId");
            DropColumn("dbo.blobFiles", "UserId");
            AlterTableAnnotations(
                "dbo.AbpOrganizationUnits",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        ParentId = c.Long(),
                        Code = c.String(nullable: false, maxLength: 128),
                        DisplayName = c.String(nullable: false, maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_Organization_MayHaveTenant",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                    { 
                        "DynamicFilter_Organization_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
        }
    }
}
