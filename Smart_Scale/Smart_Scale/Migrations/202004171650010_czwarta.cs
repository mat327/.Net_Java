namespace Smart_Scale.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class czwarta : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Pomiars", "Waga", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pomiars", "Waga", c => c.Double(nullable: false));
        }
    }
}
