using System;

namespace MPWebAPI.Models
{
    public class PhaseConfig
    {
        public int Id { get; set; }
        public DateTime StartOffset { get; set; }
        public DateTime EndOffset { get; set; }

        public int ProjectPhaseId { get; set; }
        public ProjectPhase ProjectPhase { get; set; }
        public int ProjectConfigId { get; set; }
        public ProjectConfig ProjectConfig { get; set; }
    }
}