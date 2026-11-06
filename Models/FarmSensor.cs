
using System.ComponentModel.DataAnnotations;
    namespace AgriStreamMVC.Models
{
    public class FarmSensor
    {
        public int Id { get; set; }
        [Required]
        public string SensorName { get; set; } = string.Empty;
        [Required]
        public string Location { get; set; } = string.Empty;
        [Required]
        public double ReadingValue { get; set; }
        public string? ImageUrl { get; set; }
    }
}
