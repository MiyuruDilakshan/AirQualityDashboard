namespace AirQualityDashboard.Models
{
    public class SensorWithAqi
    {
        public Sensor Sensor { get; set; }
        public int? LatestAqi { get; set; }
    }
}