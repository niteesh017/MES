using System.ComponentModel.DataAnnotations.Schema;

namespace RadialGraderMES.Models
{
    public class ProductionBatch
    {
        public int Id { get; set; }
        public string BatchNumber { get; set; } = string.Empty;
        public int ProductionOrderId { get; set; }
        public ProductionOrder? ProductionOrder { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Status { get; set; } = "Pending";
        public decimal ProducedQuantity { get; set; }
        public decimal GoodQuantity { get; set; }
        [Column("BadQuantity")]
        public decimal RejectQuantity { get; set; }
    }
}
