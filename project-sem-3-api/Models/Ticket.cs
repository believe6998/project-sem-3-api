using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace project_sem_3_api.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public long IdOrder { get; set; }
        public int IdSource { get; set; }
        public int IdDestination { get; set; }
        public int IdTrainCar { get; set; }
        public int IdSeat { get; set; }
        public int IdObjectPassenger { get; set; }
        public string PassengerName{ get; set; }
        public string IdentityNumber { get; set; }
        public decimal Price { get; set; }
        public string DepartureDay { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int Status { get; set; } = 1;
    }
}