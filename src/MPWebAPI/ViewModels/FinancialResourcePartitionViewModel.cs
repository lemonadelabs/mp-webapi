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

        public override Task MapToViewModelAsync(object model, IMerlinPlanRepository repo = null)
        {
            base.MapToViewModelAsync(model, repo);
            var frp = (FinancialResourcePartition) model;
            Categories = frp.Categories.Select(c => c.FinancialResourceCategory.Name).ToList();
            Adjustments = frp.Adjustments.Select(a => new FinancialAdjustmentViewModel(a)).ToList();
            return Task.CompletedTask;
        }

        public int Id { get; set; }
        public int FinancialResourceId { get; set; }
        public List<string> Categories { get; set; }
        public List<FinancialAdjustmentViewModel> Adjustments { get; set; }
    }
}