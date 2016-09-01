using System;

namespace MPWebAPI.Models
{
    /// <summary>
    /// Represents fund transactions from an external source.
    /// </summary>
    public class FinancialAdjustment
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public bool Additive { get; set; }
        public DateTime Date { get; set; }
        public bool Actual { get; set; }
        
        public int FinancialResourcePartitionId { get; set; }
        public FinancialResourcePartition FinancialResourcePartition { get; set; }
    }
}