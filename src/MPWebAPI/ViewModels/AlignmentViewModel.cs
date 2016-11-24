using System;
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

        public int Id { get; set; }

        public float Value { get; set; }
        public DateTime Date { get; set; }
        public bool Actual { get; set; }

        public AlignmentCategoryData AlignmentCategory { get; set; }

        public class AlignmentCategoryData
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public ProjectBenefitData ProjectBenefit { get; set; }

        public class ProjectBenefitData
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }


}