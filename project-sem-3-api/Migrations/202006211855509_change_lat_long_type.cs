namespace project_sem_3_api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_lat_long_type : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Stations", "Longitudes", c => c.String());
            AlterColumn("dbo.Stations", "Latitudes", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Stations", "Latitudes", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Stations", "Longitudes", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
