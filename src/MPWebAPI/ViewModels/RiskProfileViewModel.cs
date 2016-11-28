using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public sealed class RiskProfileViewModel : ViewModel
    {
        public RiskProfileViewModel(RiskProfile model)
        {
            MapToViewModelAsync(model);
        }

        public RiskProfileViewModel()
        {
        }

        public override Task<ViewModelMapResult> MapToViewModelAsync(object model, IMerlinPlanRepository repo = null)
        {
            var rp = (RiskProfile) model;
            base.MapToViewModelAsync(model, repo);
            RiskCategory = new RiskCategoryData
            {
                Id = rp.RiskCategoryId,
                Name = rp.RiskCategory.Name
            };
            return Task.FromResult(new ViewModelMapResult());
        }

        public override Task<ViewModelMapResult> MapToModel(object model, IMerlinPlanRepository repo = null)
        {
            if(repo == null) throw new ArgumentNullException(nameof(repo));
            var result = new ViewModelMapResult();
            var riskProfile = (RiskProfile) model;
            base.MapToModel(model, repo);
            if (repo.ProjectOptions.All(po => po.Id != ProjectOptionId))
            {
                result.AddError("ProjectOptionId", $"A project option with id {ProjectOptionId} cannot be found.");
            }
            var riskCategory = repo.RiskCategories.SingleOrDefault(rc => rc.Id == RiskCategory?.Id);
            if(riskCategory == null) result.AddError("RiskCategoryId", $"A risk category matching the id {RiskCategory?.Id} cannot be found.");
            riskProfile.RiskCategory = riskCategory;
            return Task.FromResult(result);
        }

        public int Id { get; set; }

        [Range(0.0, 1.0)]
        public float Probability { get; set; }

        public float Impact { get; set; }
        public float Mitigation { get; set; }
        public float Residual { get; set; }

        public bool Actual { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int ProjectOptionId { get; set; }

        [Required]
        public RiskCategoryData RiskCategory { get; set; }
        
        public class RiskCategoryData
        {
            [Required]
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }


}