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

        public override Task<ViewModelMapResult> MapToViewModelAsync(object model, IMerlinPlanRepository repo = null)
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
            return Task.FromResult(new ViewModelMapResult());
        }

        public override async Task<ViewModelMapResult> MapToModel(object model, IMerlinPlanRepository repo = null)
        {
            if(repo == null) throw new ArgumentNullException(nameof(repo));
            var result = await base.MapToModel(model, repo);
            var project = (Project) model;
            project.Creator = repo.Users.SingleOrDefault(u => u.Id == Creator);
            project.Group = repo.Groups.SingleOrDefault(g => g.Id == Group);

            if (project.Group == null)
            {
                result.AddError("Group", $"The Group with id {Group} can't be found");
                return result;
            }

            var frcs = Categories
                .Select(category => repo.FinancialResourceCategories
                    .SingleOrDefault(f => f.Name == category && f.GroupId == Group) ?? new FinancialResourceCategory()
                        {
                            Group = project.Group, Name = category,
                        })
                .ToList();

            project.FinancialResourceCategories = frcs.Select(frc => new ProjectFinancialResourceCategory
            {
                Project = project,
                FinancialResourceCategory = frc
            }
            ).ToList();

            project.OwningBusinessUnit = repo.BusinessUnits.SingleOrDefault(
                bu => bu.Name == OwningBusinessUnit && 
                bu.OrganisationId == project.Group.OrganisationId
            );

            project.ImpactedBusinessUnit = repo.BusinessUnits.SingleOrDefault(
                bu => bu.Name == ImpactedBusinessUnit &&
                bu.OrganisationId == project.Group.OrganisationId
            );
            return result;
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Reference { get; set; }
        public string[] Categories { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        [Required]
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
