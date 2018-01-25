namespace ScreenLocker.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial_schema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ControlNumber = c.String(nullable: false, maxLength: 8, storeType: "nvarchar"),
                        FirstName = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        MiddleName = c.String(maxLength: 50, storeType: "nvarchar"),
                        LastName = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        SecondLastName = c.String(maxLength: 50, storeType: "nvarchar"),
                        IsActive = c.Boolean(nullable: false),
                        LastLogin = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UsageLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false, precision: 0),
                        StudentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.StudentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsageLogs", "StudentId", "dbo.Students");
            DropIndex("dbo.UsageLogs", new[] { "StudentId" });
            DropTable("dbo.UsageLogs");
            DropTable("dbo.Students");
        }
    }
}
