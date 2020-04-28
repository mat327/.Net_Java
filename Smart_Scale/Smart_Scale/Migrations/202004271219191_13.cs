namespace Smart_Scale.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _13 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Pomiars", "Waga", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pomiars", "Waga", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
