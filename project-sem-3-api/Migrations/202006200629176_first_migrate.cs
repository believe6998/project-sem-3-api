namespace project_sem_3_api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first_migrate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SeatPrices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdSeat = c.Int(nullable: false),
                        Code = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false),
                        Repeat = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Seats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        CurrentPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IdTrainCarType = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Stations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Name = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TrainCars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        IdTrain = c.Int(nullable: false),
                        IdTrainCarType = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TrainCarTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        IdTrain = c.Int(nullable: false),
                        Name = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(nullable: false),
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
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(nullable: false),
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
                        Index = c.Int(nullable: false),
                        Time = c.Long(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(nullable: false),
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
                        Date = c.DateTime(nullable: false),
                        Repeat = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(nullable: false),
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
            DropTable("dbo.Stations");
            DropTable("dbo.Seats");
            DropTable("dbo.SeatPrices");
        }
    }
}
