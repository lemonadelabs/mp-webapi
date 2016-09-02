using System;
using System.Collections.Generic;

namespace MPWebAPI.Models
{
    public class ProjectBenefit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Achieved { get; set; }
        public float AchievedValue { get; set; }
        public DateTime Date { get; set; }

        public List<Alignment> Alignments { get; set; }
        public List<ProjectBenefitBenefitCategory> Categories { get; set; }

        public int ProjectOptionId { get; set; }
        public ProjectOption ProjectOption { get; set; }
    }

    public class ProjectBenefitBenefitCategory
    {
        public int ProjectBenefitId { get; set; }
        public ProjectBenefit ProjectBenefit { get; set; }

        public int BenefitCategoryId { get; set; }
        public BenefitCategory BenefitCategory { get; set; }
    }
}