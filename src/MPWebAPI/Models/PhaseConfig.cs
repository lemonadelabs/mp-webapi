using System;

namespace MPWebAPI.Models
{
    /// <summary>
    /// Stores a particular phase configuation for a
    /// particular project.
    /// </summary>
    public class PhaseConfig
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int ProjectPhaseId { get; set; }
        public ProjectPhase ProjectPhase { get; set; }

        public int ProjectConfigId { get; set; }
        public ProjectConfig ProjectConfig { get; set; }
    }
}