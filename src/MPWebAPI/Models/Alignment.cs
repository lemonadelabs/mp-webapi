using System;

namespace MPWebAPI.Models
{
    /// <summary>
    /// Represnets an alignment value for a particular project benefit
    /// tracked over time.
    /// </summary>
    public class Alignment
    {
        public int Id { get; set; }
        public float Weight { get; set; }
        public float Value { get; set; }
        public DateTime Date { get; set; }
        public bool Actual { get; set; }

        public int AlignmentCategoryId { get; set; }
        public AlignmentCategory AlignmentCategory { get; set; }

        public int ProjectBenefitId { get; set; }
        public ProjectBenefit ProjectBenefit { get; set; }
    }
}