using System.Collections.Generic;

namespace MPWebAPI.Models
{
    public class FinancialResourcePartition
    {
        public int Id { get; set; }
        public List<FinancialResourceCategory> Categories { get; set; }
        public List<FinancialAdjustment> Adjustments { get; set; }
        public decimal Size { get; set; }

        public int FinancialResourceId { get; set; }
        public FinancialResource FinancialResource { get; set; }
    }
}