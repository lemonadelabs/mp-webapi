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
    }
}