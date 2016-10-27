using System;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public sealed class FinancialAdjustmentViewModel : ViewModel
    {
        public FinancialAdjustmentViewModel(FinancialAdjustment model)
        {
            MapToViewModelAsync(model);
        }
        public int Id { get; set; }
        public decimal Value { get; set; }
        public bool Additive { get; set; }
        public DateTime Date { get; set; }
        public bool Actual { get; set; }
        public int FinancialResourcePartitionId { get; set; }
    }
}