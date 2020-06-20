using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project_sem_3_api.Models
{
    public class SeatPrice
    {
        public int Id { get; set; }
        public int IdSeat { get; set; }
        public string Code { get; set; }
        public decimal Price  { get; set; }
        public DateTime Date{ get; set; }
        public int Repeat { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public int Status { get; set; }
    }
}