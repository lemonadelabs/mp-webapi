using System;

namespace MPWebAPI.Models
{
    public class StaffTransaction
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public bool Additive { get; set; }
        public DateTime Date { get; set; }
        public bool Actual { get; set; }

        public int ProjectPhaseId { get; set; }
        public ProjectPhase ProjectPhase { get; set; }
    }
}