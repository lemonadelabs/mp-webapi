using System;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public sealed class StaffAdjustmentViewModel : ViewModel
    {
        public StaffAdjustmentViewModel(StaffAdjustment model)
        {
            MapToViewModelAsync(model);
        }

        public int Id { get; set; }
        
        public float Value { get; set; }
        public bool Additive { get; set; }
        public DateTime Date { get; set; }
        public bool Actual { get; set; }

        public int StaffResourceId { get; set; }
    }
}
