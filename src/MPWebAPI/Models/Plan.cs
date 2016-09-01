using System;
using System.Collections.Generic;

namespace MPWebAPI.Models
{
    /// <summary>
    /// A plan is a configuration of projects and project phases in time. 
    /// </summary>
    public class Plan
    {
        public int Id { get; set; }
        
        public int CreatorId { get; set; }
        public MerlinPlanUser Creator { get; set; }
        
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
        public List<PlanUser> ShareUser { get; set; }
        public List<ProjectConfig> Projects { get; set; }
    }
    
    public class PlanUser
    {
        public int PlanId { get; set; }
        public Plan Plan { get; set; }

        public int UserId { get; set; }
        public MerlinPlanUser User { get; set; }
    }
    
}