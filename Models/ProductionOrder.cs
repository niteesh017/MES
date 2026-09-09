namespace RadialGraderMES.Models
{
    public class ProductionOrder
    {
        public  int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal TargetQuantity { get; set; }
        public string Unit { get; set; } = "KG";
        public string Status { get; set; } = "Pending";
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int MachineId { get; set; }
        public  Machine? Machine { get; set; }
        public ICollection<ProductionBatch> Batches { get; set; } = new List<ProductionBatch>();
    }
}
