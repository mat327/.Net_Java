namespace Smart_Scale.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ósma : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Pomiars", "Waga", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pomiars", "Waga", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
