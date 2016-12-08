using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public sealed class PortfolioViewModel : DocumentViewModel<Portfolio, PortfolioUser>, IValidatableObject
    {

        public PortfolioViewModel(Portfolio model)
        {
            MapToViewModelAsync(model);
        }

        public PortfolioViewModel()
        {
        }

        public override Task<ViewModelMapResult> MapToViewModelAsync(Portfolio model, IMerlinPlanRepository repo = null)
        {
            base.MapToViewModelAsync(model, repo);
            ApprovedBy = model.ApprovedBy?.UserName;
            Tags = model.PortfolioTags?.Select(pt => pt.Name).ToArray() ?? new string[] {};
            return Task.FromResult(new ViewModelMapResult());
        }

        public int Id { get; set; }

        [Required]
        public DateTime StartYear { get; set; }

        [Required]
        public DateTime EndYear { get; set; }

        [Required]
        public int TimeScale { get; set; }

        public bool Approved { get; set; }
        public string ApprovedBy { get; set; }

        public string[] Tags { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Enddate must be later than start date
            if (EndYear < StartYear)
            {
                yield return new ValidationResult(
                    "EndYear should not be before StartYear",
                    new [] {"EndYear"}
                );
            }
        }
    }
}