using System;
using System.Collections.Generic;

namespace MPWebAPI.Models
{
    /// <summary>
    /// Financial Resources represent budgets or funding resource
    /// constraints.
    /// </summary>
    public class FinancialResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<FinancialResourcePartition> Partitions { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Recurring { get; set; }

        public int ResourceScenarioId { get; set; }
        public ResourceScenario ResourceScenario { get; set; }
    }
}