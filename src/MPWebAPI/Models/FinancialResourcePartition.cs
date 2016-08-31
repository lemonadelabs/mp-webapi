using System.Collections.Generic;

namespace MPWebAPI.Models
{
    /// <summary>
    /// Represents some ringfenced funding. Only projects with
    /// matching FinancialResourceCategories can access it.
    /// </summary>
    public class FinancialResourcePartition
    {
        public int Id { get; set; }
        public List<PartitionResourceCategory> Categories { get; set; }
        public List<FinancialAdjustment> Adjustments { get; set; }
        public decimal Size { get; set; }

        public int FinancialResourceId { get; set; }
        public FinancialResource FinancialResource { get; set; }
    }

    public class PartitionResourceCategory
    {
        public int FinancialResourcePartitionId { get; set; }
        public FinancialResourcePartition FinancialResourcePartition { get; set; }

        public int FinancialResourceCategoryId { get; set; }
        public FinancialResourceCategory FinancialResourceCategory { get; set; }
    }
}