namespace ClockBlock.GUI.Models.DTOs
{
    public class ConfigurationDto
    {
        public int Id { get; set; }
        public required string WorkingHoursStart { get; set; }
        public required string WorkingHoursEnd { get; set; }
    }
}
