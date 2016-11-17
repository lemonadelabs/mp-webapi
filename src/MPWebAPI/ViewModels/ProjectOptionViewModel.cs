using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public sealed class ProjectOptionViewModel : ViewModel
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(0.0, 1.0)]
        public float Priority { get; set; }

        [Range(0.0, 1.0)]
        public float Complexity { get; set; }

        public List<ProjectPhaseViewModel> Phases { get; set; }
        public List<Dependency> Dependencies { get; set; }

        public class Dependency
        {
            public int ProjectId { get; set; }
            [Required]
            public int OptionId { get; set; }
        }

        public ProjectOptionViewModel()
        {
        }

        public ProjectOptionViewModel(ProjectOption model)
        {
            MapToViewModelAsync(model);
        }

        public override async Task<ViewModelMapResult> MapToModel(object model, IMerlinPlanRepository repo = null)
        {
            var result = new ViewModelMapResult();
            if (repo == null) throw new ArgumentNullException(nameof(repo));
            
            // We need to update the dependencies
            await base.MapToModel(model, repo);
            var option = (ProjectOption) model;

            var currentDeps = option.RequiredBy?.Select(d => d.DependsOnId).ToImmutableHashSet();
            currentDeps = currentDeps ?? ImmutableHashSet<int>.Empty;
            var updatedDeps = Dependencies?.Select(d => d.OptionId).ToImmutableHashSet();
            updatedDeps = updatedDeps ?? ImmutableHashSet<int>.Empty;

            var depsToDelete = currentDeps.Where(d => !updatedDeps.Contains(d)).ToList();
            var depsToAdd = updatedDeps.Where(d => !currentDeps.Contains(d)).ToList();
            
            // Validate

            foreach (var d in depsToAdd)
            {
                if (repo.ProjectOptions.All(po => po.Id != d))
                {
                    result.AddError("Dependencies", $"A project option with the id: {d} does not exist");
                }
            }

            foreach (var d in depsToDelete)
            {
                if (repo.ProjectOptions.All(po => po.Id != d))
                {
                    result.AddError("Dependencies", $"A project option with the id: {d} does not exist");
                }
            }

            if (!result.Succeeded) return result;

            // Add

            foreach (var d in depsToAdd)
            {
                var target = repo.ProjectOptions.Single(o => o.Id == d);
                await repo.AddProjectDependencyAsync(option, target);
            }

            // Remove

            foreach (var d in depsToDelete)
            {
                var target = repo.ProjectOptions.Single(o => o.Id == d);
                await repo.RemoveProjectDependencyAsync(option, target);
            }                        

            return result;
        }

        public override Task<ViewModelMapResult> MapToViewModelAsync(object model, IMerlinPlanRepository repo = null)
        {
            base.MapToViewModelAsync(model, repo);
            var po = (ProjectOption) model;
            Phases = po.Phases?.Select(pp => new ProjectPhaseViewModel(pp)).ToList();
            Dependencies = po.RequiredBy?.Select(
                    d => new Dependency
                    {
                        OptionId = d.DependsOnId,
                        ProjectId = d.DependsOn.ProjectId
                    }
                )?.ToList();
            return Task.FromResult(new ViewModelMapResult());
        }
    }
}
