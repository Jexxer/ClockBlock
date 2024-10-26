namespace ClockBlock.GUI.Models.DTOs
{
    public class BlockEventDto
    {
        public int Id { get; set; }
        public DateTime BlockedTime { get; set; }
        public required string ApplicationName { get; set; }
    }
}
