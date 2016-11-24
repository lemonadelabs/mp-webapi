using System;
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


        public int Id { get; set; }
        public float Probability { get; set; }
        public float Impact { get; set; }
        public float Mitigation { get; set; }
        public float Residual { get; set; }
        public bool Actual { get; set; }
        public DateTime Date { get; set; }
        public int ProjectOptionId { get; set; }
        public RiskCategoryData RiskCategory { get; set; }
        
        public class RiskCategoryData
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }


}