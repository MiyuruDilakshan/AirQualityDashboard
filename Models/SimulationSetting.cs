using System.ComponentModel.DataAnnotations;

namespace AirQualityDashboard.Models
{
    public class SimulationSetting
    {
        public int Id { get; set; }

        public int FrequencyMinutes { get; set; } = 10;
        public int BaselineAQI { get; set; } = 50;
        public int Variation { get; set; } = 20;
        public bool IsRunning { get; set; } = false;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
