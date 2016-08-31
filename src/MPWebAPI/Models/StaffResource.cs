using System;
using System.Collections.Generic;

namespace MPWebAPI.Models
{
    public class StaffResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float FteOutput { get; set; }
        public List<StaffResourceCategory> Categories { get; set; }
        public List<FinancialAdjustment> Adjustments { get; set; }

        public int ResourceScenarioId { get; set; }
        public ResourceScenario ResourceScenario { get; set; }
    }
}