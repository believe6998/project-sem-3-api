using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project_sem_3_api.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal CurrentPrice { get; set; }
        public int IdTrainCarType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public int Status { get; set; }
    }
}