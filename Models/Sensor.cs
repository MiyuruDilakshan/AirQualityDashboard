using System.ComponentModel.DataAnnotations;

namespace AirQualityDashboard.Models
{
    public class Sensor
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string SensorIdentifier { get; set; }

        [Required]
        public decimal Latitude { get; set; }

        [Required]
        public decimal Longitude { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
