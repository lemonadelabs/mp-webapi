using System;
using System.Collections.Generic;

namespace MPWebAPI.Models
{
    public class ProjectPhase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<FinancialTransaction> FinancialResources { get; set; }
        public List<StaffTransaction> StaffResources { get; set; }
        public List<FinancialTransaction> Revenue { get; set; }
        public List<ProjectBenefit> Benefits { get; set; }
    }
}