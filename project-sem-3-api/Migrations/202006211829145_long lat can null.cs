namespace project_sem_3_api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class longlatcannull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Stations", "Longitudes", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Stations", "Latitudes", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.report", "Status",
                c => c.Int(nullable: false, defaultValue: 1));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Stations", "Latitudes", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Stations", "Longitudes", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
