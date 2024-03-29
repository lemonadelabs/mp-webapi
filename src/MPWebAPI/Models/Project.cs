using System;
using System.Collections.Generic;

namespace MPWebAPI.Models
{
    public class Project : IMerlinPlanDocument<ProjectUser>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Reference { get; set; }
        public List<ProjectFinancialResourceCategory> FinancialResourceCategories { get; set; }
        
        public MerlinPlanUser Creator { get; set; }
        public Group Group { get; set; }
        
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public bool ShareAll { get; set; }
        public bool ShareGroup { get; set; }
        public List<ProjectUser> ShareUser { get; set; }

        public int? OwningBusinessUnitId { get; set; }
        public BusinessUnit OwningBusinessUnit { get; set; }

        public int? ImpactedBusinessUnitId { get; set; }
        public BusinessUnit ImpactedBusinessUnit { get; set; }

        public List<ProjectOption> Options { get; set; }
    }

    public class ProjectFinancialResourceCategory
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public int FinancialResourceCategoryId { get; set; }
        public FinancialResourceCategory FinancialResourceCategory { get; set; }
    }

    public class ProjectUser : IDocumentUser
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public string UserId { get; set; }
        public MerlinPlanUser User { get; set; }
    }
}