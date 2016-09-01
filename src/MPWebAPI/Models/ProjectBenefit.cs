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
        public DateTime Date { get; set; }

        public List<Alignment> Alignments { get; set; }
        public int ProjectOptionId { get; set; }
        public ProjectOption ProjectOption { get; set; }
    }
}