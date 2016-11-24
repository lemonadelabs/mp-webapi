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
            return Task.FromResult(new ViewModelMapResult());
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

    public sealed class BenefitCategoryViewModel : ViewModel
    {
        public BenefitCategoryViewModel(BenefitCategory model)
        {
            MapToViewModelAsync(model);
        }

        public BenefitCategoryViewModel()
        {
        }

        public override Task<ViewModelMapResult> MapToViewModelAsync(object model, IMerlinPlanRepository repo = null)
        {
            var bc = (BenefitCategory) model;
            base.MapToViewModelAsync(model, repo);
            Group = new GroupData
            {
                Id = bc.GroupId,
                Name = bc.Group.Name
            };
            return Task.FromResult(new ViewModelMapResult());
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public GroupData Group { get; set; }
    }


    public sealed class RiskCategoryViewModel : ViewModel
    {
        public RiskCategoryViewModel(RiskCategory model)
        {
            MapToViewModelAsync(model);
        }

        public RiskCategoryViewModel()
        {
        }


        public override Task<ViewModelMapResult> MapToViewModelAsync(object model, IMerlinPlanRepository repo = null)
        {
            var rc = (RiskCategory) model;
            base.MapToViewModelAsync(model, repo);
            Group = new GroupData
            {
                Id = rc.GroupId,
                Name = rc.Group.Name
            };
            return Task.FromResult(new ViewModelMapResult());
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public float Bias { get; set; }
        public GroupData Group { get; set; }
    }

    public sealed class AlignmentCategoryViewModel : ViewModel
    {
        public AlignmentCategoryViewModel(AlignmentCategory model)
        {
            MapToViewModelAsync(model);
        }

        public AlignmentCategoryViewModel()
        {
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Area { get; set; }
        public float Weight { get; set; }
        public float RiskBias { get; set; }
        public GroupData Group { get; set; }

        public override Task<ViewModelMapResult> MapToViewModelAsync(object model, IMerlinPlanRepository repo = null)
        {
            var ac = (AlignmentCategory) model;
            base.MapToViewModelAsync(model, repo);
            Group = new GroupData
            {
                Id = ac.Group.Id,
                Name = ac.Group.Name
            };
            return Task.FromResult(new ViewModelMapResult());
        }
    }

    public class GroupData
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}