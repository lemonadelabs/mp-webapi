using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public class StaffResourceViewModel : ViewModel, IValidatableObject
    {
        public StaffResourceViewModel(StaffResource model)
        {
            MapToViewModel(model);
        }

        public StaffResourceViewModel() {}

        public int Id { get; set; }
        public int ResourceScenarioId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public List<string> Categories { get; set; }
        public UserViewModel UserData { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // EndDate cant be before StartDate
            if (EndDate <= StartDate)
            {
                yield return new ValidationResult(
                    "EndDate should not be before StartDate",
                    new [] {"EndDate"}
                );
            }
        }
    }
}