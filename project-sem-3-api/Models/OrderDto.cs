using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project_sem_3_api.Models
{
    public class OrderDto
    {
        public long Id { get; set; }
        public decimal TotalPrice { get; set; }
        public string LinkPaymentPaypal { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public List<TicketDto> TicketDtos { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int Status { get; set; } = 1;

        public String Expired
        {
            get
            {
                return CreatedAt.AddDays(1).ToString("dd-MM-yyyy hh:mm:ss");
            }
        }

        public String StatusString
        {
            get
            {
                switch (this.Status)
                {
                    case 0:
                        return "Cancel";
                    case 1:
                        return "Pending";
                    case 2:
                        return "Done";
                    default:
                        return null;
                }
            }
        }
    }

    public class TicketDto
    {
        public string Code { get; set; }
        public long IdOrder { get; set; }
        public string CodeTrain { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string TrainCarNumber { get; set; }
        public string TrainCarType { get; set; }
        public string SeatNumber { get; set; }
        public string ObjectPassenger { get; set; }
        public string PassengerName { get; set; }
        public string IdentityNumber { get; set; }
        public decimal Price { get; set; }
        public string DepartureDay { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int Status { get; set; } = 1;

    }
}