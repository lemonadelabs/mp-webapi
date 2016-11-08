using System;
using System.Collections.Generic;

namespace MPWebAPI.Models
{
    public interface IMerlinPlanDocument
    {
        MerlinPlanUser Creator { get; set; }
        Group Group { get; set; }
        DateTime Created { get; set; }
        DateTime Modified { get; set; }
        bool ShareAll { get; set; }
        bool ShareGroup { get; set; }
    }
}