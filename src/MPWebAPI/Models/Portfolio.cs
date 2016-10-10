using System;
using System.Collections.Generic;

namespace MPWebAPI.Models
{
    /// <summary>
    /// A plan is a configuration of projects and project phases in time. 
    /// </summary>
    public class Portfolio
    {
        public int Id { get; set; }
        
        public MerlinPlanUser Creator { get; set; }
        public Group Group { get; set; }
        
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string Name { get; set; }
        public DateTime StartYear { get; set; }
        public DateTime EndYear { get; set; }
        public int TimeScale { get; set; }
        public bool ShareAll { get; set; }
        public bool ShareGroup { get; set; }
        public bool Approved { get; set; }
        public MerlinPlanUser ApprovedBy { get; set; }
        public List<PortfolioUser> ShareUser { get; set; }
        public List<ProjectConfig> Projects { get; set; }
    }
    
    public class PortfolioUser
    {
        public int PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; }

        public string UserId { get; set; }
        public MerlinPlanUser User { get; set; }
    }
    
}