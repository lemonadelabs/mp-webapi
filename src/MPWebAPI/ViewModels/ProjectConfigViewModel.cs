using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public sealed class ProjectConfigViewModel : ViewModel, IValidatableObject
    {

        public ProjectConfigViewModel(ProjectConfig model)
        {
            MapToViewModelAsync(model);
        }

        public ProjectConfigViewModel()
        {
        }

        public int Id { get; set; }
        public PhaseConfigViewModel[] Phases { get; set; }

        [Required]
        public int PortfolioId { get; set; }
        public UserInfo Owner { get; set; }
        public UserInfo[] Managers { get; set; }

        [Required]
        public int ProjectOptionId { get; set; }

        public string[] Tags { get; set; }

        public class UserInfo
        {
            public string FirstName { get; set; }

            [Required]
            public int Id { get; set; }
        }

        public override async Task<ViewModelMapResult> MapToModel(object model, IMerlinPlanRepository repo = null)
        {
            if(repo == null) throw new ArgumentNullException(nameof(repo));
            var result = new ViewModelMapResult();
            await base.MapToModel(model, repo);
            var projectConfig = (ProjectConfig) model;
            projectConfig.Owner = repo.StaffResources.SingleOrDefault(sr => sr.Id == Owner?.Id);
            projectConfig.OwnerId = projectConfig.Owner?.Id;

            // Update Managers
            if (Managers == null)
                Managers = new UserInfo[] {};

            if(projectConfig.Managers == null)
                projectConfig.Managers = new List<StaffResourceProjectConfig>();

            var newManagers =
                Managers
                    .Where(m => !projectConfig.Managers.Select(ma => ma.StaffResourceId).Contains(m.Id))
                    .Select(mid => mid.Id)
                    .ToList();

            var managersToDelete =
                projectConfig.Managers
                    .Where(m => !Managers.Select(ma => ma.Id).Contains(m.StaffResourceId))
                    .Select(mid => mid.StaffResourceId)
                    .ToList();

            await repo.RemoveManagersFromProjectConfigAsync(projectConfig, managersToDelete);
            await repo.AddManagersToProjectConfigAsync(projectConfig, newManagers);

            // Update Phases
            // Find valid phases
            foreach (var phase in Phases)
            {
                var phaseToUpdate = projectConfig.Phases.SingleOrDefault(p => p.Id == phase.Id);
                if(phaseToUpdate == null) continue;
                phaseToUpdate.EndDate = phase.EndDate;
                phaseToUpdate.StartDate = phase.StartDate;
            }

            // Update Tags
            var tagsToAdd = Tags
                .Where(t => !projectConfig.Tags.Select(ta => ta.PortfolioTag.Name).Contains(t))
                .ToList();

            var tagsToDelete =
                projectConfig.Tags
                    .Select(t => t.PortfolioTag.Name)
                    .Where(tn => !Tags.Contains(tn))
                    .ToList();

            await repo.RemoveTagsFromProjectConfigAsync(projectConfig, tagsToDelete);

            foreach (var tag in tagsToAdd)
            {
                await repo.AddTagToPortfolioAsync(projectConfig.Portfolio, tag);
            }
            await repo.AddTagsToProjectConfigAsync(projectConfig, tagsToAdd);
            return result;
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

            Tags = projectConfig.Tags?.Select(tag => tag.PortfolioTag.Name).ToArray() ?? new string[] {};
            return Task.FromResult(new ViewModelMapResult());
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Check for overlapping phases
            return from phase in Phases where Phases.Any(p =>
                (phase.StartDate > p.StartDate && phase.StartDate < p.EndDate && phase != p) ||
                (phase.EndDate > p.StartDate && phase.EndDate < p.EndDate && phase != p))
                select new ValidationResult($"Phase ( id ={phase.Id} ) overlaps with another phase", new [] {"StartDate", "EndDate"});
        }
    }

    public sealed class PhaseConfigViewModel : ViewModel, IValidatableObject
    {
        public PhaseConfigViewModel(PhaseConfig model)
        {
            MapToViewModelAsync(model);
        }

        public PhaseConfigViewModel()
        {
        }

        public int Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public int ProjectPhaseId { get; set; }

        [Required]
        public int ProjectConfigId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Enddate must be later than start date
            if (EndDate < StartDate)
            {
                yield return new ValidationResult(
                    "EndDate should not be before StartDate",
                    new [] {"Endate"}
                );
            }
        }
    }
}