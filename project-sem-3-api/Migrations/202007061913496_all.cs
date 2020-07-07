namespace project_sem_3_api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class all : DbMigration
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
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Email = c.String(),
                        Name = c.String(),
                        Phone = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        DeletedAt = c.DateTime(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Seats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SeatNo = c.Int(nullable: false),
                        IdSeatType = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        DeletedAt = c.DateTime(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SeatTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdTrainCarType = c.Int(nullable: false),
                        Name = c.String(),
                        Price = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        DeletedAt = c.DateTime(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Stations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Location = c.Geography(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        DeletedAt = c.DateTime(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        IdOrder = c.Long(nullable: false),
                        IdSource = c.Int(nullable: false),
                        IdDestination = c.Int(nullable: false),
                        IdTrainCar = c.Int(nullable: false),
                        IdSeat = c.Int(nullable: false),
                        IdObjectPassenger = c.Int(nullable: false),
                        PassengerName = c.String(),
                        IdentityNumber = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DepartureDay = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        DeletedAt = c.DateTime(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TrainCars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdTrain = c.Int(nullable: false),
                        IdTrainCarType = c.Int(nullable: false),
                        IndexNumber = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        DeletedAt = c.DateTime(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TrainCarTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Name = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        DeletedAt = c.DateTime(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Trains",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        DeletedAt = c.DateTime(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TrainStations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdTrain = c.Int(nullable: false),
                        IdStation = c.Int(nullable: false),
                        IndexNumber = c.Int(nullable: false),
                        ArrivalTime = c.Long(nullable: false),
                        DistancePreStation = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        DeletedAt = c.DateTime(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TrainTrainCars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdTrain = c.Int(nullable: false),
                        IdTrainCar = c.Int(nullable: false),
                        Date = c.String(),
                        Repeat = c.Int(nullable: false),
                        PricePercent = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        DeletedAt = c.DateTime(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TrainTrainCars");
            DropTable("dbo.TrainStations");
            DropTable("dbo.Trains");
            DropTable("dbo.TrainCarTypes");
            DropTable("dbo.TrainCars");
            DropTable("dbo.Tickets");
            DropTable("dbo.Stations");
            DropTable("dbo.SeatTypes");
            DropTable("dbo.Seats");
            DropTable("dbo.Orders");
            DropTable("dbo.ObjectPassengers");
        }
    }
}
