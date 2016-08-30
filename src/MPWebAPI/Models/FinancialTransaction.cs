using System;
using System.Collections.Generic;

namespace MPWebAPI.Models
{
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
}