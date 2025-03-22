using System.ComponentModel.DataAnnotations.Schema;

namespace AirQualityDashboard.Models // Ensure correct namespace
{
    public class SensorReading
    {
        public int Id { get; set; }
        public int SensorId { get; set; }
        public int AQI { get; set; }
        public DateTime RecordedAt { get; set; }

        // Navigation property
        public virtual Sensor Sensor { get; set; } = null!;
    }
}