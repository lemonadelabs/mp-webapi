using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public sealed class AlignmentViewModel : ViewModel
    {
        public AlignmentViewModel(Alignment model)
        {
            MapToViewModelAsync(model);
        }

        public AlignmentViewModel()
        {
        }

        public override Task<ViewModelMapResult> MapToViewModelAsync(object model, IMerlinPlanRepository repo = null)
        {
            var a = (Alignment) model;
            base.MapToViewModelAsync(model, repo);
            AlignmentCategory = new AlignmentCategoryData
            {
                Id = a.AlignmentCategoryId,
                Name = a.AlignmentCategory?.Name
            };

            ProjectBenefit = new ProjectBenefitData
            {
                Id = a.ProjectBenefitId,
                Name = a.ProjectBenefit?.Name
            };
            return Task.FromResult(new ViewModelMapResult());
        }

        public override Task<ViewModelMapResult> MapToModel(object model, IMerlinPlanRepository repo = null)
        {
            if(repo == null) throw new ArgumentNullException(nameof(repo));
            var result = new ViewModelMapResult();

            var alignment = (Alignment) model;

            base.MapToModel(model, repo);

            var alignmentCategory = repo.AlignmentCategories.SingleOrDefault(ac => ac.Id == AlignmentCategory?.Id);
            if(alignmentCategory == null) result.AddError("AlignmentCategoryId", $"An alignment category with id {AlignmentCategory?.Id} could not be found.");

            var projectBenefit = repo.ProjectBenefits.SingleOrDefault(pb => pb.Id == ProjectBenefit?.Id);
            if(projectBenefit == null) result.AddError("ProjectBenefitId", $"A project benefit with id {ProjectBenefit?.Id} could not be found");

            alignment.AlignmentCategory = alignmentCategory;
            alignment.ProjectBenefit = projectBenefit;

            return Task.FromResult(result);
        }

        public int Id { get; set; }

        public float Value { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public bool Actual { get; set; }

        [Required]
        public AlignmentCategoryData AlignmentCategory { get; set; }

        public class AlignmentCategoryData
        {
            [Required]
            public int Id { get; set; }
            public string Name { get; set; }
        }

        [Required]
        public ProjectBenefitData ProjectBenefit { get; set; }

        public class ProjectBenefitData
        {
            [Required]
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}