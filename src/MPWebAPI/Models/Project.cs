using System.Collections.Generic;

namespace MPWebAPI.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Reference { get; set; }
        public List<FinancialResourceCategory> FinancialResourceCategories { get; set; }
        
        public StaffResource Owner { get; set; }
        public List<StaffResource> Managers { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
        public List<ProjectOption> Options { get; set; }
    }
}