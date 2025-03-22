using AirQualityDashboard.Data;
using AirQualityDashboard.Models;
using Microsoft.EntityFrameworkCore;

public class SimulationService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly TimeSpan _checkInterval = TimeSpan.FromSeconds(30);

    public SimulationService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AirQualityDbContext>();
                var simulationSetting = await context.SimulationSettings.FirstOrDefaultAsync();

                if (simulationSetting != null && simulationSetting.IsRunning)
                {
                    // Fetch all active sensors
                    var sensors = await context.Sensors.Where(s => s.IsActive).ToListAsync();
                    var random = new Random();
                    foreach (var sensor in sensors)
                    {
                        // Generate a simulated AQI value using baseline and variation.
                        int aqi = simulationSetting.BaselineAQI + random.Next(-simulationSetting.Variation, simulationSetting.Variation);
                        var aqiData = new AQIData { SensorId = sensor.Id, AQI = aqi };
                        var now = DateTime.UtcNow;
                        var existingAqi = await context.AQIData.FirstOrDefaultAsync(a => a.SensorId == sensor.Id);
                        if (existingAqi != null)
                        {
                            existingAqi.AQI = aqi;
                            existingAqi.RecordedAt = now;
                            context.AQIData.Update(existingAqi);
                        }
                        else
                        {
                            var newAQIData = new AQIData
                            {
                                SensorId = sensor.Id,
                                AQI = aqi,
                                RecordedAt = now
                            };
                            context.AQIData.Add(newAQIData);
                        }

                        // Also add a historical sensor reading:
                        var sensorReading = new SensorReading
                        {
                            SensorId = sensor.Id,
                            AQI = aqi,
                            RecordedAt = now
                        };
                        context.SensorReadings.Add(sensorReading);
                    }
                    await context.SaveChangesAsync();
                }
            }
            await Task.Delay(_checkInterval, stoppingToken);
        }
    }
}