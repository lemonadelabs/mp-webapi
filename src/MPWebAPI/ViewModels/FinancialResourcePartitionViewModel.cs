using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public sealed class FinancialResourcePartitionViewModel : ViewModel
    {
        public FinancialResourcePartitionViewModel(FinancialResourcePartition model)
        {
            MapToViewModelAsync(model);
        }

        public FinancialResourcePartitionViewModel()
        {
        }

        public override Task<ViewModelMapResult> MapToModel(object model, IMerlinPlanRepository repo = null)
        {
            base.MapToModel(model, repo);

            if (!Value.HasValue) return Task.FromResult(new ViewModelMapResult());
            var frp = (FinancialResourcePartition) model;
            var adjustment = frp.Adjustments.OrderBy(a => a.Date).FirstOrDefault();
            if (adjustment == null) return Task.FromResult(new ViewModelMapResult());
            adjustment.Value = Value.Value;
            return Task.FromResult(new ViewModelMapResult());
        }

        public override Task<ViewModelMapResult> MapToViewModelAsync(object model, IMerlinPlanRepository repo = null)
        {
            base.MapToViewModelAsync(model, repo);
            var frp = (FinancialResourcePartition) model;
            Categories = frp.Categories.Select(c => c.FinancialResourceCategory.Name).ToList();
            var adjustment = frp.Adjustments.OrderBy(a => a.Date).FirstOrDefault();
            if (adjustment != null)
            {
                Value = adjustment.Value;
            }
            return Task.FromResult(new ViewModelMapResult());
        }

        public int Id { get; set; }
        public int FinancialResourceId { get; set; }
        public List<string> Categories { get; set; }
        public decimal? Value { get; set; }
    }
}