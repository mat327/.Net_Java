namespace Smart_Scale.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pomiars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Waga = c.String(),
                        Datadodania = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Imie = c.String(),
                        Nazwisko = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pomiars", "UserId", "dbo.Users");
            DropIndex("dbo.Pomiars", new[] { "UserId" });
            DropTable("dbo.Users");
            DropTable("dbo.Pomiars");
        }
    }
}
