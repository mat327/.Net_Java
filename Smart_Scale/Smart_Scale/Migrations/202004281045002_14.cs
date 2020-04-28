namespace Smart_Scale.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _14 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Plec", c => c.String(nullable: false, maxLength: 1));
            AddColumn("dbo.Users", "Wzrost", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "Wiek", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Wiek");
            DropColumn("dbo.Users", "Wzrost");
            DropColumn("dbo.Users", "Plec");
        }
    }
}
