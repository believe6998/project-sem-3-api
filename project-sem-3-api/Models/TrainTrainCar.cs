using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace project_sem_3_api.Models
{
    public class TrainTrainCar
    {
        public int Id { get; set; }
        public int IdTrain { get; set; }
        public int IdTrainCar { get; set; }
        public string Date { get; set; }
        [Range(1,7)]
        public int Repeat { get; set; }
        public int PricePercent { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int Status { get; set; } = 1;
    }
}