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
        public float? FteOutput { get; set; }
        public List<StaffResourceStaffResourceCategory> Categories { get; set; }
        public List<FinancialAdjustment> Adjustments { get; set; }

        public int ResourceScenarioId { get; set; }
        public ResourceScenario ResourceScenario { get; set; }

        public List<Project> ProjectsOwned { get; set; }
        public List<StaffResourceProject> ProjectsManaged { get; set; }
        public MerlinPlanUser UserData { get; set; }
    }

    public class StaffResourceStaffResourceCategory
    {
        public int StaffResourceId { get; set; }
        public StaffResource StaffResource { get; set; }

        public int StaffResourceCategoryId { get; set; }
        public StaffResourceCategory StaffResourceCategory { get; set; }
    }
}