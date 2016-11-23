using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public sealed class ProjectBenefitViewModel : ViewModel
    {
        public int Id { get; set; }

        [Required]
        public int ProjectOptionId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool Achieved { get; set; }
        public float? AchievedValue { get; set; }
        public DateTime? Date { get; set; }

        public string[] Categories { get; set; }


        public ProjectBenefitViewModel(ProjectBenefit model)
        {
            MapToViewModelAsync(model);
        }

        public ProjectBenefitViewModel()
        {
        }

        public override Task<ViewModelMapResult> MapToViewModelAsync(object model, IMerlinPlanRepository repo = null)
        {
            var result = base.MapToViewModelAsync(model, repo);
            var benefit = (ProjectBenefit) model;
            // Map Categories
            Categories = benefit.Categories?.Select(c => c.BenefitCategory.Name).ToArray();
            return result;
        }
    }
}