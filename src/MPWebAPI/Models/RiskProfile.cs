using System;

namespace MPWebAPI.Models
{
    public class RiskProfile
    {
        public int Id { get; set; }
        public float Probability { get; set; }
        public float Impact { get; set; }
        public float Mitigation { get; set; }
        public float Residual { get; set; }
        public bool Actual { get; set; }
        public DateTime Date { get; set; }

        public int ProjectOptionId { get; set; }
        public ProjectOption ProjectOption { get; set; }

        public int RiskCategoryId { get; set; }
        public RiskCategory RiskCategory { get; set; }
    }
}