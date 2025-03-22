using System.ComponentModel.DataAnnotations;

namespace AirQualityDashboard.Models
{
    public class Alert
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string AQILevel { get; set; }   // e.g., "Moderate", "Unhealthy"

        public int Threshold { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
