using System;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public class ResourceScenarioViewModel : ViewModel
    {
        public ResourceScenarioViewModel(ResourceScenario model)
        {
            MapToViewModel(model);
            Creator = model.Creator.UserName;
            ApprovedBy = model.ApprovedBy?.UserName;
            Group = model.Group.Id;
        }
        
        public int Id { get; set; }
        public int Group { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string Creator { get; set; }
        public bool Approved { get; set; }
        public string ApprovedBy { get; set; }
    }
}