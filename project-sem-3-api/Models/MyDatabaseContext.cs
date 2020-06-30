using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using project_sem_3_api.Models;

namespace project_sem_3_api.Models
{
    public class MyDatabaseContext : DbContext
    {
        public MyDatabaseContext()
            : base("MyConnectionString")
        {
        }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Train> Trains { get; set; }
        public DbSet<TrainStation> TrainStations { get; set; }
        public DbSet<TrainCar> TrainCars { get; set; }
        public DbSet<TrainTrainCar> TrainTrainCars { get; set; }
        public DbSet<TrainCarType> TrainCarTypes { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<SeatType> SeatTypes { get; set; }
        public DbSet<ObjectPassenger> ObjectPassengers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

    }
    
}