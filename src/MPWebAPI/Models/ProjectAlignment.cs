using System;

namespace MPWebAPI.Models
{
    public class ProjectAlignment
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