using System.Collections.Generic;
using OpenIddict;

namespace MPWebAPI.Models
{
    public class MerlinPlanUser : OpenIddictUser
    {
        public List<Plan> Plans { get; set; }
        public List<ResourceScenario> ResourceScenarios { get; set; }
        public List<Project> Projects { get; set; }
        
        public int OrganisationId { get; set; }
        public Organisation Organisation { get; set; }
        
        public int GroupId { get; set; } 
        public Group Group { get; set; }
    }
}
