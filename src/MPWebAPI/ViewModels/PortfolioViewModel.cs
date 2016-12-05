using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public sealed class PortfolioViewModel : ViewModel, IValidatableObject
    {

        // TODO: Refactor common document view properties into a base class.
        // TODO: Move common fiel classes into a common base class
        public PortfolioViewModel(Portfolio model)
        {
            MapToViewModelAsync(model);
        }

        public PortfolioViewModel()
        {
        }

        public override Task<ViewModelMapResult> MapToViewModelAsync(object model, IMerlinPlanRepository repo = null)
        {
            var portfolio = (Portfolio) model;
            base.MapToViewModelAsync(model, repo);
            ApprovedBy = portfolio.ApprovedBy?.UserName;
            Group = portfolio.Group.Id;
            Creator = portfolio.Creator.Id;
            CreatorDetails = new UserDetails
            {
                FirstName = portfolio.Creator.FirstName,
                LastName = portfolio.Creator.LastName
            };
            return Task.FromResult(new ViewModelMapResult());
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime StartYear { get; set; }

        [Required]
        public DateTime EndYear { get; set; }

        [Required]
        public int TimeScale { get; set; }

        public bool Approved { get; set; }
        public string ApprovedBy { get; set; }


        [Required]
        public int Group { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        [Required]
        public string Creator { get; set; }
        public UserDetails CreatorDetails { get; set; }

        public class UserDetails
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

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