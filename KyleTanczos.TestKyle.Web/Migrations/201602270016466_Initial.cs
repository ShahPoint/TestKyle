namespace KyleTanczos.TestKyle.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OutComeTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OutcomeName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UploadedFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatedBy = c.String(),
                        RawXml = c.String(),
                        FileName = c.String(),
                        StartDateRange = c.DateTime(nullable: false),
                        EndDateRange = c.DateTime(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
            DropIndex("dbo.UploadedPcrs", new[] { "UploadedFile_Id" });
            DropIndex("dbo.UploadedPcrs", new[] { "outcome_Id" });
            DropTable("dbo.UploadedPcrs");
            DropTable("dbo.UploadedFiles");
            DropTable("dbo.OutComeTypes");
        }
    }
}
