using System;
using System.Linq;
using System.Threading.Tasks;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public sealed class ProjectViewModel : DocumentViewModel<Project, ProjectUser>
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

            Categories = project.FinancialResourceCategories
                .Select(frc => frc.FinancialResourceCategory.Name)
                .ToArray();

            OwningBusinessUnit = project.OwningBusinessUnit?.Name;
            ImpactedBusinessUnit = project.ImpactedBusinessUnit?.Name;
            return Task.FromResult(new ViewModelMapResult());
        }

        public override async Task<ViewModelMapResult> MapToModel(Project model, IMerlinPlanRepository repo = null)
        {
            if(repo == null) throw new ArgumentNullException(nameof(repo));
            var result = await base.MapToModel(model, repo);

            var frcs = Categories
                .Select(category => repo.FinancialResourceCategories
                    .SingleOrDefault(f => f.Name == category && f.GroupId == Group) ?? new FinancialResourceCategory()
                        {
                            Group = model.Group, Name = category,
                        })
                .ToList();

            model.FinancialResourceCategories = frcs.Select(frc => new ProjectFinancialResourceCategory
            {
                Project = model,
                FinancialResourceCategory = frc
            }
            ).ToList();

            model.OwningBusinessUnit = repo.BusinessUnits.SingleOrDefault(
                bu => bu.Name == OwningBusinessUnit && 
                bu.OrganisationId == model.Group.OrganisationId
            );

            model.ImpactedBusinessUnit = repo.BusinessUnits.SingleOrDefault(
                bu => bu.Name == ImpactedBusinessUnit &&
                bu.OrganisationId == model.Group.OrganisationId
            );
            return result;
        }

        public int Id { get; set; }

        public string Summary { get; set; }
        public string Reference { get; set; }
        public string[] Categories { get; set; }

        public string OwningBusinessUnit { get; set; }
        public string ImpactedBusinessUnit { get; set; }
    }
}
