using AirQualityDashboard.Data;
using AirQualityDashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirQualityDashboard.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : Controller
    {
        private readonly AirQualityDbContext _context;

        public AdminDashboardController(AirQualityDbContext context)
        {
            _context = context;
        }

        // GET: /AdminDashboard/Index
        public async Task<IActionResult> Index()
        {
            // Example dashboard data: count active sensors and simulation status.
            var activeSensors = await _context.Sensors.CountAsync(s => s.IsActive);
            var simulation = await _context.SimulationSettings.FirstOrDefaultAsync();
            ViewBag.ActiveSensors = activeSensors;
            ViewBag.SimulationStatus = simulation?.IsRunning ?? false;
            return View();
        }

        #region Sensor Management

        // GET: /AdminDashboard/Sensors
        public async Task<IActionResult> Sensors()
        {
            var sensors = await _context.Sensors.ToListAsync();
            return View(sensors);
        }

        // GET: /AdminDashboard/CreateSensor
        public IActionResult CreateSensor()
        {
            return View();
        }

        // POST: /AdminDashboard/CreateSensor
        [HttpPost]
        public async Task<IActionResult> CreateSensor(Sensor sensor)
        {
            if (ModelState.IsValid)
            {
                _context.Sensors.Add(sensor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Sensors));
            }
            return View(sensor);
        }

        // GET: /AdminDashboard/EditSensor/5
        public async Task<IActionResult> EditSensor(int id)
        {
            var sensor = await _context.Sensors.FindAsync(id);
            if (sensor == null) return NotFound();
            return View(sensor);
        }

        // POST: /AdminDashboard/EditSensor/5
        // Update the EditSensor POST method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSensor(int id, Sensor sensor)
        {
            if (id != sensor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingSensor = await _context.Sensors.FindAsync(id);
                    if (existingSensor == null)
                    {
                        return NotFound();
                    }

                    existingSensor.SensorIdentifier = sensor.SensorIdentifier;
                    existingSensor.Latitude = sensor.Latitude;
                    existingSensor.Longitude = sensor.Longitude;
                    existingSensor.IsActive = sensor.IsActive;

                    _context.Update(existingSensor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SensorExists(sensor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Sensors));
            }
            return View(sensor);
        }

        // New Toggle Status method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleSensorStatus(int id)
        {
            try
            {
                var sensor = await _context.Sensors.FindAsync(id);
                if (sensor == null)
                {
                    return NotFound();
                }

                sensor.IsActive = !sensor.IsActive;
                _context.Update(sensor);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Sensor {sensor.SensorIdentifier} status updated successfully";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating sensor status: {ex.Message}";
            }

            return RedirectToAction(nameof(Sensors));
        }

        private bool SensorExists(int id)
        {
            return _context.Sensors.Any(e => e.Id == id);
        }

        // POST: /AdminDashboard/DeactivateSensor/5
        [HttpPost]
        public async Task<IActionResult> DeactivateSensor(int id)
        {
            var sensor = await _context.Sensors.FindAsync(id);
            if (sensor != null)
            {
                sensor.IsActive = false;
                _context.Update(sensor);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Sensors));
        }
        #endregion

        #region Simulation Management

        // GET: /AdminDashboard/SimulationSettings
        public async Task<IActionResult> SimulationSettings()
        {
            var simulation = await _context.SimulationSettings.FirstOrDefaultAsync();
            if (simulation == null)
            {
                simulation = new SimulationSetting();
                _context.SimulationSettings.Add(simulation);
                await _context.SaveChangesAsync();
            }
            return View(simulation);
        }

        // POST: /AdminDashboard/SimulationSettings
        [HttpPost]
        public async Task<IActionResult> SimulationSettings(SimulationSetting settings)
        {
            if (ModelState.IsValid)
            {
                _context.SimulationSettings.Update(settings);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(settings);
        }

        // POST: /AdminDashboard/ToggleSimulation
        [HttpPost]
        public async Task<IActionResult> ToggleSimulation()
        {
            var simulation = await _context.SimulationSettings.FirstOrDefaultAsync();
            if (simulation != null)
            {
                simulation.IsRunning = !simulation.IsRunning;
                _context.Update(simulation);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(SimulationSettings));
        }
        #endregion

        #region Alert Configuration

        // GET: /AdminDashboard/Alerts
        public async Task<IActionResult> Alerts()
        {
            var alerts = await _context.Alerts.ToListAsync();
            return View(alerts);
        }

        // GET: /AdminDashboard/CreateAlert
        public IActionResult CreateAlert()
        {
            return View();
        }

        // POST: /AdminDashboard/CreateAlert
        [HttpPost]
        public async Task<IActionResult> CreateAlert(Alert alert)
        {
            if (ModelState.IsValid)
            {
                _context.Alerts.Add(alert);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Alerts));
            }
            return View(alert);
        }
        #endregion

        #region User Management

        // GET: /AdminDashboard/Users
        public async Task<IActionResult> Users()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        // GET: /AdminDashboard/CreateUser
        public IActionResult CreateUser()
        {
            return View();
        }

        // POST: /AdminDashboard/CreateUser
        [HttpPost]
        public async Task<IActionResult> CreateUser(User user, string password)
        {
            if (ModelState.IsValid)
            {
                // For demonstration, we hash the password simply.
                user.PasswordHash = ComputeSha256Hash(password);
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Users));
            }
            return View(user);
        }

        private string ComputeSha256Hash(string rawData)
        {
            using (var sha256Hash = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(rawData));
                var builder = new System.Text.StringBuilder();
                foreach (var t in bytes)
                    builder.Append(t.ToString("x2"));
                return builder.ToString();
            }
        }
        #endregion
    }
}