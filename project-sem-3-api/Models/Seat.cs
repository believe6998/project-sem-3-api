using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project_sem_3_api.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public int SeatNo { get; set; }
        public int IdSeatType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int Status { get; set; } = 1;
    }
}