using System.ComponentModel.DataAnnotations;

namespace AirQualityDashboard.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        [Required]
        public string Role { get; set; } = "Admin";

        // Add FullName property
        public string FullName { get; set; } = null!;
    }
}