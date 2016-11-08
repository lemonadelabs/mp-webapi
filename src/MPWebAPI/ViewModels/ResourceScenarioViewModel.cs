using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public sealed class ResourceScenarioViewModel : ViewModel
    {
        public ResourceScenarioViewModel(ResourceScenario model)
        {
            MapToViewModelAsync(model);
        }

        public override Task MapToViewModelAsync(object model, IMerlinPlanRepository repo = null)
        {
            base.MapToViewModelAsync(model, repo);
            var rs = (ResourceScenario) model;
            Creator = rs.Creator.Id;
            ApprovedBy = rs.ApprovedBy?.UserName;
            Group = rs.Group.Id;
            CreatorDetails = new UserDetails
            {
                FirstName = rs.Creator.FirstName,
                LastName = rs.Creator.LastName
            };
            return Task.CompletedTask;
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

        public UserDetails CreatorDetails { get; set; }
        
        public bool Approved { get; set; }
        public string ApprovedBy { get; set; }


        public class UserDetails
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
    }
}