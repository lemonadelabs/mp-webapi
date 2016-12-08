using System.Threading.Tasks;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public sealed class ResourceScenarioViewModel : DocumentViewModel<ResourceScenario, ResourceScenarioUser>
    {
        public ResourceScenarioViewModel(ResourceScenario model)
        {
            MapToViewModelAsync(model);
        }

        public override Task<ViewModelMapResult> MapToViewModelAsync(ResourceScenario model, IMerlinPlanRepository repo = null)
        {
            base.MapToViewModelAsync(model, repo);
            ApprovedBy = model.ApprovedBy?.UserName;
            return new Task<ViewModelMapResult>(() => new ViewModelMapResult());
        }
        public ResourceScenarioViewModel() {}
        
        public int Id { get; set; }
        public bool Approved { get; set; }
        public string ApprovedBy { get; set; }
    }
}