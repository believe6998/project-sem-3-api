namespace project_sem_3_api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addlonglat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stations", "Longitudes", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Stations", "Latitudes", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Stations", "UpdatedAt", c => c.DateTime());
            AlterColumn("dbo.Stations", "DeletedAt", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Stations", "DeletedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Stations", "UpdatedAt", c => c.DateTime(nullable: false));
            DropColumn("dbo.Stations", "Latitudes");
            DropColumn("dbo.Stations", "Longitudes");
        }
    }
}
