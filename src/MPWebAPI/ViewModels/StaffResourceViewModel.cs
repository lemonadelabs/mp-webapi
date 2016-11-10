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

        public override Task<ViewModelMapResult> MapToViewModelAsync(object model, IMerlinPlanRepository repo = null)
        {
            var sr = (StaffResource) model;
            if (sr == null) throw new InvalidCastException();
            
            base.MapToViewModelAsync(sr, repo);
            
            // add categories
            Categories = sr.Categories
                .Select(src => src.StaffResourceCategory.Name)
                .ToList();
            
            // Add value from adjustment
            var first = sr.Adjustments.OrderBy(a => a.Date).FirstOrDefault();
            Value = first?.Value ?? 0f;

            return Task.FromResult(new ViewModelMapResult());
        }

        public override Task<ViewModelMapResult> MapToModel(object model, IMerlinPlanRepository repo = null)
        {
            base.MapToModel(model, repo);

            var resource = (StaffResource)model;

            // Map Value to first adjustment
            if (Value.HasValue)
            {
                var adjustment = resource.Adjustments.OrderBy(a => a.Date).FirstOrDefault();
                if (adjustment != null)
                {
                    adjustment.Value = Value.Value;
                }
            }
            
            // Map categories
            if (Categories == null || repo == null) throw new ArgumentNullException();
            
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
            return new Task<ViewModelMapResult>(() => new ViewModelMapResult());
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
        public bool Recurring { get; set; }

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