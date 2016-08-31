using System;
using System.Collections.Generic;

namespace MPWebAPI.Models
{
    /// <summary>
    /// Represents a flow of resources in time between a
    /// project and a financial resource.
    /// </summary>
    public class FinancialTransaction
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public bool Additive { get; set; }
        public DateTime Date { get; set; }
        public bool Actual { get; set; }
        public string Reference { get; set; }
        public List<FinancialResourceCategory> Categories { get; set; }
        
        public int ProjectPhaseId { get; set; }
        public ProjectPhase ProjectPhase { get; set; }
    }

    /// <summary>
    /// Many to many join between FinancialTransaction and FinancialResourceCategory
    /// </summary>
    public class FinancialTransactionResourceCategory
    {
        public int FinancialResourceCategoryId { get; set; }
        public FinancialResourceCategory FinancialResourceCategory { get; set; }

        public int FinancialTransactionId { get; set; }
        public FinancialTransaction FinancialTransaction { get; set; }
    }
}