using System.Collections.Generic;

namespace MPWebAPI.Models
{
    /// <summary>
    /// Represents a specifc version of a project with its
    /// own alignments, phases and risk profile.
    /// </summary>
    public class ProjectOption
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public float Priority { get; set; }
        public float Complexity { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public List<ProjectPhase> Phases { get; set; }
        public List<RiskProfile> RiskProfile { get; set; }
        public List<ProjectBenefit> Benefits { get; set; }
        public List<ProjectDependency> Dependencies {get; set;}
        public List<ProjectDependency> RequiredBy { get; set; }
    }

    public class ProjectDependency
    {
        public int DependsOnId { get; set; }
        public ProjectOption DependsOn { get; set; }

        public int RequiredById { get; set; }
        public ProjectOption RequiredBy { get; set; }
    }

}