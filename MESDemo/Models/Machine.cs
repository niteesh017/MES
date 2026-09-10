namespace RadialGraderMES.Models
{
    public class Machine
    {
        public int Id { get; set; }

        public string MachineCode { get; set; } = string.Empty;

        public string MachineName { get; set; } = string.Empty;

        public string Status { get; set; } = "Stopped";

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}