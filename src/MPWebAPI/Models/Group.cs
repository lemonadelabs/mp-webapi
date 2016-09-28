using System.Collections.Generic;

namespace MPWebAPI.Models
{
    /// <summary>
    /// Represents work-group within an organisation. 
    /// Each group in Merlin: Plan has its own categories and users.
    /// </summary>
    public class Group
    {
        public Group()
        {
            Active = true;
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public int? ParentId { get; set; }
        public Group Parent { get; set; }

        public bool Active { get; set; }
        
        public List<Group> Children { get; set; }
        public List<UserGroup> Members { get; set; }

        public List<ResourceScenario> ResourceScenarios { get; set; }
        public List<Project> Projects { get; set; }
        public List<Portfolio> Portfolios { get; set; }

        public List<AlignmentCategory> AlignmentCategories { get; set; }
        public List<StaffResourceCategory> StaffResourceCategories { get; set; }
        public List<FinancialResourceCategory> FinancialResourceCategories { get; set; }
        public List<RiskCategory> RiskCategories { get; set; }
        public List<BenefitCategory> BenefitCategories { get; set; }
        
        public int OrganisationId { get; set; }
        public Organisation Organisation { get; set; }
    }
}