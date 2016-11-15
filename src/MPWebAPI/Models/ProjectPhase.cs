using System;
using System.Collections.Generic;

namespace MPWebAPI.Models
{
    /// <summary>
    /// A project phase is a stage of a project with its own
    /// finantial and staffing constraints.
    /// </summary>
    public class ProjectPhase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EstimatedStartDate { get; set; }
        public DateTime EstimatedEndDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<FinancialTransaction> FinancialResources { get; set; }
        public List<StaffTransaction> StaffResources { get; set; }

        public int ProjectOptionId { get; set; }
        public ProjectOption ProjectOption { get; set; }
    }
}