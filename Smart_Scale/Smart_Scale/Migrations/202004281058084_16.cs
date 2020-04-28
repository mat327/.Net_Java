namespace Smart_Scale.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _16 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pomiars", "Bmi", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pomiars", "Bmi");
        }
    }
}
