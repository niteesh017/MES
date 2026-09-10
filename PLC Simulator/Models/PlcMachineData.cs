namespace PLC_Simulator.Models
{
    public class PlcMachineData
    {
        public string MachineCode { get; set; } = "RG-001";
        public string MachineStatus { get; set; } = "Running";

        public decimal MachineSpeed { get; set; }

        public decimal InputWeight { get; set; }
        public decimal OutputWeight { get; set; }

        public decimal Grade1Weight { get; set; }
        public decimal Grade2Weight { get; set; }
        public decimal Grade3Weight { get; set; }
        public decimal Grade4Weight { get; set; }

        public decimal TotalProduction { get; set; }

        public decimal Quality { get; set; }
        public decimal OEE { get; set; }

        public decimal ProcessTemperature { get; set; }
        public string MachineHealth { get; set; } = "Healthy";
        public int DowntimeSeconds { get; set; }
        public decimal ProductionRate { get; set; }

        public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
    }
}