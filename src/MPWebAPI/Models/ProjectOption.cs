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
        public List<ProjectAlignment> Alignments { get; set; }
        public List<RiskProfile> RiskProfile { get; set; }
        public List<ProjectBenefit> Benefits { get; set; }
    }
}