namespace KyleTanczos.TestKyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JayObjectsmiration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.blobFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        fileContents2 = c.Binary(),
                        byteCount = c.Int(nullable: false),
                        created = c.DateTime(nullable: false),
                        fileName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NemsisDataElements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FieldNumber = c.String(),
                        FieldName = c.String(),
                        OptionCode = c.String(),
                        OptionText = c.String(),
                        State = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OutComeTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OutcomeName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Select2OptionsList",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ControlName = c.String(),
                        oldJsListName = c.String(),
                        Association = c.String(),
                        OptionsAsJson = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UploadedFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatedBy = c.String(),
                        RawFile = c.String(unicode: false, storeType: "text"),
                        RawXml = c.String(),
                        FileName = c.String(),
                        StartDateRange = c.DateTime(nullable: false),
                        EndDateRange = c.DateTime(nullable: false),
                        Count = c.Int(nullable: false),
                        file_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.blobFiles", t => t.file_Id)
                .Index(t => t.file_Id);
            
            CreateTable(
                "dbo.UploadedPcrs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IncidentDate = c.DateTime(nullable: false),
                        outcome_Id = c.Int(),
                        UploadedFile_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OutComeTypes", t => t.outcome_Id)
                .ForeignKey("dbo.UploadedFiles", t => t.UploadedFile_Id)
                .Index(t => t.outcome_Id)
                .Index(t => t.UploadedFile_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UploadedPcrs", "UploadedFile_Id", "dbo.UploadedFiles");
            DropForeignKey("dbo.UploadedPcrs", "outcome_Id", "dbo.OutComeTypes");
            DropForeignKey("dbo.UploadedFiles", "file_Id", "dbo.blobFiles");
            DropIndex("dbo.UploadedPcrs", new[] { "UploadedFile_Id" });
            DropIndex("dbo.UploadedPcrs", new[] { "outcome_Id" });
            DropIndex("dbo.UploadedFiles", new[] { "file_Id" });
            DropTable("dbo.UploadedPcrs");
            DropTable("dbo.UploadedFiles");
            DropTable("dbo.Select2OptionsList");
            DropTable("dbo.OutComeTypes");
            DropTable("dbo.NemsisDataElements");
            DropTable("dbo.blobFiles");
        }
    }
}
