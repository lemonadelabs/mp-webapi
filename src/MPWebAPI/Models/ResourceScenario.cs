using System;
using System.Collections.Generic;

namespace MPWebAPI.Models
{
    public class ResourceScenario
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public MerlinPlanUser Creator { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public bool ShareAll { get; set; }
        public bool ShareGroup { get; set; }
        public List<MerlinPlanUser> ShareUser { get; set; }
    }
}