﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project_sem_3_api.Models
{
    public class TrainStation
    {
        public int Id { get; set; }
        public int IdTrain { get; set; }
        public int IdStation { get; set; }
        public int Index { get; set; }
        public long Time { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public int Status { get; set; }
    }
}