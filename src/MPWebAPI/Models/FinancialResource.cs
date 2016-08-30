using System;
using System.Collections.Generic;

namespace MPWebAPI.Models
{
    public class FinancialResource
    {
        public string Name { get; set; }
        public List<FinancialResourcePartition> Partitions { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}