using System;

namespace MPWebAPI.Models
{
    public class ProjectBenefit
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public int Description { get; set; }
        public bool Achieved { get; set; }
        public DateTime Date { get; set; }

        public int ProjectPhaseId { get; set; }
        public ProjectPhase ProjectPhase { get; set; }
    }
}