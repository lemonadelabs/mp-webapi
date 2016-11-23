using System;
using System.Collections.Generic;
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

        public override async Task<ViewModelMapResult> MapToModel(object model, IMerlinPlanRepository repo = null)
        {
            if (repo == null) throw new ArgumentNullException(nameof(repo));

            var benefit = (ProjectBenefit) model;
            var result = new ViewModelMapResult();
            await base.MapToModel(model, repo);

            // Check project option Id is valid
            var option = repo.ProjectOptions.SingleOrDefault(po => po.Id == ProjectOptionId);
            if (option == null)
            {
                result.AddError("ProjectOptionId", $"{ProjectOptionId} is not a valid Id");
                return result;
            }

            // Map Categories
            // check that supplied categories are valid

            var categories = new List<BenefitCategory>();
            foreach (var category in Categories)
            {
                var bc = repo.BenefitCategories
                    .Where(c => c.GroupId == option.Project.Group.Id)
                    .SingleOrDefault(ca => ca.Name == category);

                if (bc != null)
                {
                    categories.Add(bc);
                    continue;
                }
                result.AddError("Categories", $"The category {category} is not valid.");
            }

            if (!result.Succeeded) return result;
            benefit.Categories = categories
                .Select(c => new ProjectBenefitBenefitCategory
                    {
                        BenefitCategory = c,
                        ProjectBenefit = benefit
                    }).
                ToList();
            return result;
        }
    }
}