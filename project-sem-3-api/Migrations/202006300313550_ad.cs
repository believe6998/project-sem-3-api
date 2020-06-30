namespace project_sem_3_api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ad : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ObjectPassengers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PricePercent = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Orders", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "UpdatedAt", c => c.DateTime());
            AddColumn("dbo.Orders", "DeletedAt", c => c.DateTime());
            AddColumn("dbo.Orders", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Tickets", "IdOrder", c => c.Int(nullable: false));
            AddColumn("dbo.Tickets", "IdObjectPassenger", c => c.Int(nullable: false));
            AddColumn("dbo.Tickets", "PassengerName", c => c.String());
            DropColumn("dbo.TrainStations", "DistancePreStation");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TrainStations", "DistancePreStation", c => c.Int(nullable: false));
            DropColumn("dbo.Tickets", "PassengerName");
            DropColumn("dbo.Tickets", "IdObjectPassenger");
            DropColumn("dbo.Tickets", "IdOrder");
            DropColumn("dbo.Orders", "Status");
            DropColumn("dbo.Orders", "DeletedAt");
            DropColumn("dbo.Orders", "UpdatedAt");
            DropColumn("dbo.Orders", "CreatedAt");
            DropTable("dbo.ObjectPassengers");
        }
    }
}
