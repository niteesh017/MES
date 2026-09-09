namespace MESDemo.Models
{
    public class PlcDataDto
    {
        public string MachineCode  { get; set; }
        public string MachineStatus  { get; set; }
        public decimal MachineSpeed { get; set; }
        public decimal InputWeight { get; set; }
        public decimal OutputWeight { get; set; }
        public decimal Grade1Weight { get; set; }
        public decimal Grade2Weight { get; set; }
        public decimal Grade3Weight { get; set; }
        public decimal Grade4Weight { get; set; }
        public decimal TotalProduction { get; set; }
        public DateTime RecordedAt { get; set; }
    }
}
