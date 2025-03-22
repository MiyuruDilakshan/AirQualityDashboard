using Microsoft.EntityFrameworkCore;
using AirQualityDashboard.Models;


namespace AirQualityDashboard.Data
{
    public class AirQualityDbContext : DbContext
    {
        public AirQualityDbContext(DbContextOptions<AirQualityDbContext> options)
            : base(options) { }

        public DbSet<Sensor> Sensors { get; set; } = null!;
      
        public DbSet<SensorReading> SensorReadings { get; set; } = null!;
        public DbSet<SimulationSetting> SimulationSettings { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AQIData> AQIData { get; set; } = null!;



    }
}