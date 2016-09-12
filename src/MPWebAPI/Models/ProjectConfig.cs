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
        public DateTime StartDate { get; set; }
        public List<PhaseConfig> Phases { get; set; }
        
        public int PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; }
        
        public int ProjectOptionId { get; set; }
        public ProjectOption ProjectOption { get; set; }    
    }
}