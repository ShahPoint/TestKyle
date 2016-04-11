namespace KyleTanczos.TestKyle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitalJayMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RawFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileContents = c.Binary(),
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
                        Created = c.DateTime(nullable: false),
                        CreatedByUserName = c.String(),
                        CreatedByUserId = c.Int(nullable: false),
                        FileName = c.String(),
                        StartDateRange = c.DateTime(nullable: false),
                        EndDateRange = c.DateTime(nullable: false),
                        TripCount = c.Int(nullable: false),
                        ByteCount = c.Int(nullable: false),
                        RawFile_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RawFiles", t => t.RawFile_Id)
                .Index(t => t.RawFile_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UploadedFiles", "RawFile_Id", "dbo.RawFiles");
            DropIndex("dbo.UploadedFiles", new[] { "RawFile_Id" });
            DropTable("dbo.UploadedFiles");
            DropTable("dbo.Select2OptionsList");
            DropTable("dbo.NemsisDataElements");
            DropTable("dbo.RawFiles");
        }
    }
}
