using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project_sem_3_api.Models
{
    public class Station
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Longitudes  { get; set; }
        public string Latitudes  { get; set; }
        public DateTime CreatedAt
        {
            get =>
                _dateCreated ?? DateTime.Now;

            set => this._dateCreated = value;
        }

        private DateTime? _dateCreated = null;
        public DateTime? UpdatedAt  { get; set; }
        public DateTime? DeletedAt  { get; set; }
        public int Status { get ; set; }
    }
}