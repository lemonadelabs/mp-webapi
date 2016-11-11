using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public sealed class StaffTransactionViewModel : ViewModel
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public bool Additive { get; set; }
        public DateTime Date { get; set; }
        public bool Actual { get; set; }

        public string Category { get; set; }

        public StaffTransactionViewModel()
        {
        }

        public StaffTransactionViewModel(StaffTransaction model)
        {
            MapToViewModelAsync(model);
        }

        public override Task<ViewModelMapResult> MapToViewModelAsync(object model, IMerlinPlanRepository repo = null)
        {
            base.MapToViewModelAsync(model, repo);
            var st = (StaffTransaction) model;
            Category = st.Category.Name;
            return Task.FromResult(new ViewModelMapResult());
        }
    }
}
