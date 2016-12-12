using System;
using System.Collections.Generic;

namespace MPWebAPI.Models
{
    /// <summary>
    /// A specific configuration of a project option in a specific plan.
    /// </summary>
    public class ProjectConfig
    {
        public int Id { get; set; }
        public List<PhaseConfig> Phases { get; set; }
        
        public int PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; }

        public int? OwnerId { get; set; }
        public StaffResource Owner { get; set; }

        public List<StaffResourceProjectConfig> Managers { get; set; }
        
        public int ProjectOptionId { get; set; }
        public ProjectOption ProjectOption { get; set; }

        public List<ProjectConfigPortfolioTag> Tags { get; set; }
    }

    public class StaffResourceProjectConfig
    {
        public int StaffResourceId { get; set; }
        public StaffResource StaffResource { get; set; }

        public int ProjectConfigId { get; set; }
        public ProjectConfig ProjectConfig { get; set; }
    }
}