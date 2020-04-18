namespace Smart_Scale.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pierwsza : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Pomiars", "Waga", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "Imie", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "Nazwisko", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Nazwisko", c => c.String());
            AlterColumn("dbo.Users", "Imie", c => c.String());
            AlterColumn("dbo.Pomiars", "Waga", c => c.String());
        }
    }
}
