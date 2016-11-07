using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public sealed class ProjectViewModel : ViewModel
    {

        public ProjectViewModel(Project model)
        {
            MapToViewModelAsync(model);
        }

        public ProjectViewModel()
        {
        }

        public override Task MapToViewModelAsync(object model, IMerlinPlanRepository repo = null)
        {
            base.MapToViewModelAsync(model, repo);
            var project = (Project) model;
            Creator = project.Creator.Id;
            CreatorDetails = new UserDetails
            {
                LastName = project.Creator.LastName,
                FirstName = project.Creator.FirstName
            };

            Categories = project.FinancialResourceCategories
                .Select(frc => frc.FinancialResourceCategory.Name)
                .ToArray();

            Group = project.Group.Id;
            OwningBusinessUnit = project.OwningBusinessUnit?.Name;
            ImpactedBusinessUnit = project.ImpactedBusinessUnit?.Name;
            return Task.CompletedTask;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Reference { get; set; }
        public string[] Categories { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public string Creator { get; set; }
        public UserDetails CreatorDetails { get; set; }

        public string OwningBusinessUnit { get; set; }
        public string ImpactedBusinessUnit { get; set; }

        [Required]
        public int Group { get; set; }


        public class UserDetails
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
    }
}
