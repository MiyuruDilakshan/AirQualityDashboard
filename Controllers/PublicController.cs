// Controllers/PublicController.cs
using AirQualityDashboard.Data;
using AirQualityDashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AirQualityDashboard.Controllers
{
    public class PublicController : Controller
    {
        private readonly AirQualityDbContext _context;

        public PublicController(AirQualityDbContext context)
        {
            _context = context;
        }

        // GET: /Public/Index
        public async Task<IActionResult> Index()
        {
            // Clear any existing authentication
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            var activeSensors = await _context.Sensors
                .Where(s => s.IsActive)
                .ToListAsync();
            return View(activeSensors);
        }

        // GET: /Public/SensorData/{id}
        // Returns historical AQI data for a sensor.
        public async Task<IActionResult> SensorData(int id)
        {
            var data = await _context.SensorReadings
                        .Where(s => s.SensorId == id)
                        .OrderByDescending(s => s.RecordedAt)
                        .Take(24) // Get the latest 24 readings
                        .ToListAsync();
            return Json(data);
        }



        // POST: /Public/SaveAQI
        // Simulates saving a real-time AQI reading for a sensor.
        [HttpPost]
        public async Task<IActionResult> SaveAQI(int sensorId, int aqi)
        {
            // Check if an AQIData record exists for this sensor
            var existingAqi = await _context.AQIData.FirstOrDefaultAsync(a => a.SensorId == sensorId);
            if (existingAqi != null)
            {
                // Update existing record
                existingAqi.AQI = aqi;
                existingAqi.RecordedAt = DateTime.UtcNow;
                _context.AQIData.Update(existingAqi);
            }
            else
            {
                // Create new record if none exists
                var newAQIData = new AQIData
                {
                    SensorId = sensorId,
                    AQI = aqi,
                    RecordedAt = DateTime.UtcNow
                };
                _context.AQIData.Add(newAQIData);
            }

            // Save to SensorReadings for historical data
            var sensorReading = new SensorReading
            {
                SensorId = sensorId,
                AQI = aqi,
                RecordedAt = DateTime.UtcNow
            };
            _context.SensorReadings.Add(sensorReading);

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "AQI recorded successfully." });
        }


    }
}