using System.Collections.Generic;
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
