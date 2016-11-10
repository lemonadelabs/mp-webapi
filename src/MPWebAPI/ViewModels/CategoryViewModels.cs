using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public sealed class FinancialResourceCategoryViewModel : ViewModel
    {
        public FinancialResourceCategoryViewModel(FinancialResourceCategory model)
        {
            MapToViewModelAsync(model);
        }
        
        public FinancialResourceCategoryViewModel() {}
        
        public override Task<ViewModelMapResult> MapToViewModelAsync(object model, IMerlinPlanRepository repo = null)
        {
            var frc = (FinancialResourceCategory) model;
            base.MapToViewModelAsync(model, repo);
            Group = new GroupData { Id = frc.Group.Id, Name = frc.Group.Name };
            return new Task<ViewModelMapResult>(() => new ViewModelMapResult());
        }
        
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }
        public GroupData Group { get; set; }
    }

    public sealed class BusinessUnitViewModel : ViewModel
    {
        public BusinessUnitViewModel(BusinessUnit model)
        {
            MapToViewModelAsync(model);
        }

        public BusinessUnitViewModel()
        {
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        public int OrganisationId { get; set; }
    }

    public class GroupData
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}