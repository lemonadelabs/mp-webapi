using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public sealed class ProjectConfigViewModel : ViewModel
    {

        public ProjectConfigViewModel(ProjectConfig model)
        {
            MapToViewModelAsync(model);
        }

        public ProjectConfigViewModel()
        {
        }

        public int Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        public PhaseConfigViewModel[] Phases { get; set; }

        [Required]
        public int PortfolioId { get; set; }
        public UserInfo Owner { get; set; }
        public UserInfo[] Managers { get; set; }

        [Required]
        public int ProjectOptionId { get; set; }

        public class UserInfo
        {
            public string FirstName { get; set; }

            [Required]
            public int Id { get; set; }
        }

        public override Task<ViewModelMapResult> MapToViewModelAsync(object model, IMerlinPlanRepository repo = null)
        {
            var projectConfig = (ProjectConfig) model;
            base.MapToViewModelAsync(model, repo);
            Phases = projectConfig.Phases?.Select(p => new PhaseConfigViewModel(p)).ToArray();
            Owner = projectConfig.OwnerId.HasValue
                ? new UserInfo
                {
                    FirstName = projectConfig.Owner.Name,
                    Id = projectConfig.OwnerId.Value
                }
                : null;

            Managers = projectConfig.Managers?
                .Select(m => new UserInfo
            {
                FirstName = m.StaffResource.Name,
                Id = m.StaffResourceId
            })
                .ToArray();
            return Task.FromResult(new ViewModelMapResult());
        }
    }

    public sealed class PhaseConfigViewModel : ViewModel
    {
        public PhaseConfigViewModel(PhaseConfig model)
        {
            MapToViewModelAsync(model);
        }

        public PhaseConfigViewModel()
        {
        }

        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int ProjectPhaseId { get; set; }
        public int ProjectConfigId { get; set; }
    }
}