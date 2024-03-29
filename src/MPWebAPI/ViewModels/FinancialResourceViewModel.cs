using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public sealed class FinancialResourceViewModel : ViewModel, IValidatableObject
    {
        public FinancialResourceViewModel(FinancialResource model)
        {
            MapToViewModelAsync(model);
        }

        public FinancialResourceViewModel() {}

        public int Id { get; set; }

        public int ResourceScenarioId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        public bool Recurring { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Check EndDate is not before StartDate
            if (EndDate.HasValue && EndDate <= StartDate)
            {
                yield return new ValidationResult(
                    "EndDate is before StartDate",
                    new[] {"EndDate"}
                );
            }
        }
    }
}