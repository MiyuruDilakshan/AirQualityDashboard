using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirQualityDashboard.Models;

namespace AirQualityDashboard.Models
{
    public class AQIData
    {
        public int Id { get; set; }
        public int SensorId { get; set; }
        public int AQI { get; set; }
        public DateTime RecordedAt { get; set; }

        // Navigation property
        public virtual Sensor Sensor { get; set; }
    }
}
