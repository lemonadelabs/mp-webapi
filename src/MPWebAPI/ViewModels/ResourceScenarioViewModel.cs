using System;
using System.ComponentModel.DataAnnotations;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public class ResourceScenarioViewModel : ViewModel
    {
        public ResourceScenarioViewModel(ResourceScenario model)
        {
            MapToViewModelAsync(model);
            Creator = model.Creator.UserName;
            ApprovedBy = model.ApprovedBy?.UserName;
            Group = model.Group.Id;
        }

        public ResourceScenarioViewModel() {}
        
        public int Id { get; set; }
        
        [Required]
        public int Group { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        [Required]
        [EmailAddress]
        public string Creator { get; set; }
        
        public bool Approved { get; set; }
        public string ApprovedBy { get; set; }
    }
}