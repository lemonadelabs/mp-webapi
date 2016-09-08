using System;

namespace MPWebAPI.Models
{
    /// <summary>
    /// Represents staff movements from an external source.
    /// </summary>
    public class StaffAdjustment
    {
        public int Id { get; set; }
        public float Value { get; set; }
        public bool Additive { get; set; }
        public DateTime Date { get; set; }
        public bool Actual { get; set; }

        public int StaffResourceId { get; set; }
        public StaffResource StaffResource { get; set; }
    }
}