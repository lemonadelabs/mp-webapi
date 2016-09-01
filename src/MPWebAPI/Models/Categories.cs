using System.Collections.Generic;

namespace MPWebAPI.Models
{
    /// <summary>
    /// Represents an internal business unit or group in an organisation
    /// that may not nessesarily have a Group entry in the system.
    /// </summary>
    public class BusinessUnit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int OrganisationId { get; set; }
        public Organisation Organisation { get; set; }

        public List<Project> Projects { get; set; }
    }
    

    /// <summary>
    /// Represents a type of staff member. I.e 'line staff', 'project manager'
    /// etc.
    /// </summary>
    public class StaffResourceCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<StaffResourceStaffResourceCategory> StaffResources { get; set; }
    }

    /// <summary>
    /// Represents a type of funding. Used by the system to ringfence 
    /// certian funding for particular projects.
    /// </summary>
    public class FinancialResourceCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public List<ProjectFinancialResourceCategory> Projects { get; set; }
        public List<PartitionResourceCategory> FinancialPartitions { get; set; }
    }

    /// <summary>
    /// Represents a particular type of risk. Used to
    /// group risk in projects.
    /// </summary>
    public class RiskCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public List<RiskProfile> RiskProfiles { get; set; }
    }

    /// <summary>
    /// Represents a business goal or strategic direction. Project
    /// benefits are linked with Alignments. 
    /// <see cref="ProjectBenefit"/>
    /// </summary>
    public class AlignmentCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Area { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public List<Alignment> Alignments { get; set; }
    }
}