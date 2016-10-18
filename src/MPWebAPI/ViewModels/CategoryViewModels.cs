using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public class FinancialResourceCategoryViewModel : ViewModel
    {
        public FinancialResourceCategoryViewModel(FinancialResourceCategory model)
        {
            MapToViewModelAsync(model);
        }
        
        public FinancialResourceCategoryViewModel() {}
        
        public override Task MapToViewModelAsync(object model, IMerlinPlanRepository repo = null)
        {
            var frc = (FinancialResourceCategory) model;
            base.MapToViewModelAsync(model);
            Group = new GroupData { Id = frc.Group.Id, Name = frc.Group.Name };
            return Task.CompletedTask;
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public GroupData Group { get; set; }
    }

    public class GroupData
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}