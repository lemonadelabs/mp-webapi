using System.Collections.Generic;
using OpenIddict;

namespace MPWebAPI.Models
{
    /// <summary>
    /// A user of Merlin: Plan software.
    /// </summary>
    public class MerlinPlanUser : OpenIddictUser
    {
        public int OrganisationId { get; set; }
        public Organisation Organisation { get; set; }
        
        public string EmployeeId { get; set; }
        public List<UserGroup> Groups { get; set; }

        // Ownership
        public List<ResourceScenario> ResourceScenarios { get; set; }
        public List<Plan> Plans { get; set; }
        public List<Project> Projects { get; set; }

        // Sharing
        public List<ResourceScenarioUser> SharedResourceScenarios { get; set; }
        public List<PlanUser> SharedPlans { get; set; }
        public List<ProjectUser> SharedProjects { get; set; }

        // Related resource data
        public int StaffResourceId { get; set; }
        public StaffResource StaffResource { get; set; }
    }

    public class UserGroup
    {
        public int UserId { get; set; }
        public MerlinPlanUser User { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
