using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public sealed class StaffResourceViewModel : ViewModel, IValidatableObject
    {
        public StaffResourceViewModel() {}

        public StaffResourceViewModel(StaffResource model)
        {
            MapToViewModelAsync(model);
        }

        public override Task MapToViewModelAsync(object model, IMerlinPlanRepository repo = null)
        {
            var sr = (StaffResource) model;
            if (sr == null) return Task.CompletedTask;
            
            base.MapToViewModelAsync(sr, repo);
            
            // add categories
            Categories = sr.Categories
                .Select(src => src.StaffResourceCategory.Name)
                .ToList();
            
            // Add value from adjustment
            var first = sr.Adjustments.OrderBy(a => a.Date).FirstOrDefault();
            Value = first?.Value ?? 0f;

            return Task.CompletedTask;
        }

        public override Task MapToModel(object model, IMerlinPlanRepository repo = null)
        {
            base.MapToModel(model, repo);
            
            // Map categories
            if (Categories == null || repo == null) return Task.CompletedTask;
            
            var resource = (StaffResource)model;
            
            // delete
            var toDelete = resource.Categories
                .Where(rc => !Categories.Contains(rc.StaffResourceCategory.Name))
                .ToList();

            foreach (var category in toDelete)
            {
                resource.Categories.Remove(category);
            }

            var toAdd =
                Categories
                    .Where(c => !resource.Categories.Select(rc => rc.StaffResourceCategory.Name).ToList().Contains(c))
                    .ToList();

            // Add
            foreach (var category in toAdd)
            {
                // try and find existing category
                var mcat =
                    repo.StaffResourceCategories
                        .Where(rc => rc.Group.Id == resource.ResourceScenario.Group.Id)
                        .SingleOrDefault(rc => rc.Name == category) ?? new StaffResourceCategory()
                    {
                        Name = category,
                        Group = resource.ResourceScenario.Group,
                    };

                var srsrc = new StaffResourceStaffResourceCategory()
                {
                    StaffResourceCategory = mcat,
                    StaffResource = resource,
                };

                resource.Categories.Add(srsrc);
            }
            return Task.CompletedTask;
        }

        public int Id { get; set; }
        
        public int ResourceScenarioId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public List<string> Categories { get; set; }
        public UserViewModel UserData { get; set; }

        public float? Value { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // EndDate cant be before StartDate
            if (EndDate.HasValue && EndDate <= StartDate)
            {
                yield return new ValidationResult(
                    "EndDate should not be before StartDate",
                    new [] {"EndDate"}
                );
            }
        }
    }
}