using System.Collections.Generic;

namespace MPWebAPI.Models
{
    public class ProjectAlignment
    {
        public int Id { get; set; }
        public List<AlignmentWeight> Weight { get; set; }
        public List<AlignmentValue> Value { get; set; }

        public int CategoryId { get; set; }
        public AlignmentCategory Category { get; set; }
    }
}