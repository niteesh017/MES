namespace RadialGraderMES.Models
{
    public class MachineData
    {
        public long Id { get; set; }
        public int MachineId { get; set; }
        public Machine? Machine { get; set; }
        public string MachineStatus { get; set; } = "Stopped";
        public decimal MachineSpeed{ get; set; }
        public decimal InputWeight { get; set; }
        public decimal OutputWeight { get; set; }
        public decimal Grade1Weight { get; set; }
        public decimal Grade2Weight { get; set; }
        public decimal Grade3Weight { get; set; }
        public decimal Grade4Weight { get; set; }
        public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
    }
}
