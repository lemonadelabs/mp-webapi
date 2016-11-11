using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public sealed class ProjectPhaseViewModel : ViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EstimatedStartDate { get; set; }
        public DateTime EstimatedEndDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<FinancialTransactionViewModel> FinancialResources { get; set; }
        public List<StaffTransactionViewModel> StaffResources { get; set; }

        public ProjectPhaseViewModel()
        {
        }

        public ProjectPhaseViewModel(ProjectPhase model)
        {
            MapToViewModelAsync(model);
        }

        public override Task<ViewModelMapResult> MapToViewModelAsync(object model, IMerlinPlanRepository repo = null)
        {
            base.MapToViewModelAsync(model, repo);
            var pp = (ProjectPhase) model;
            FinancialResources = pp.FinancialResources.Select(ft => new FinancialTransactionViewModel(ft)).ToList();
            StaffResources = pp.StaffResources.Select(sr => new StaffTransactionViewModel(sr)).ToList();
            return Task.FromResult(new ViewModelMapResult());
        }
    }
}