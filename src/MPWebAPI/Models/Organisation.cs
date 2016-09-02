using System.Collections.Generic;

namespace MPWebAPI.Models
{
    /// <summary>
    /// The top level model in the system. Represents
    /// A single instance of Merlin: Plan. Each client would
    /// have only one instance of Organisation.
    /// </summary>
    public class Organisation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }
        public string Contact { get; set; }
        public string WebDomain { get; set; }
        public int FinancialYearStart { get; set; }
        public List<Group> Groups { get; set; }
        public List<BusinessUnit> BusinessUnits { get; set; }
        public List<MerlinPlanUser> Users { get; set; }
    }
}
