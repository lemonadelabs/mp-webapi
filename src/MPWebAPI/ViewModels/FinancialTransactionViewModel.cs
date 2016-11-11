using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public sealed class FinancialTransactionViewModel : ViewModel
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public bool Additive { get; set; }
        public DateTime Date { get; set; }
        public bool Actual { get; set; }
        public string Reference { get; set; }
        public List<string> Categories { get; set; }

        public FinancialTransactionViewModel()
        {
        }

        public FinancialTransactionViewModel(FinancialTransaction model)
        {
            MapToViewModelAsync(model);
        }

        public override Task<ViewModelMapResult> MapToViewModelAsync(object model, IMerlinPlanRepository repo = null)
        {
            base.MapToViewModelAsync(model, repo);
            var ft = (FinancialTransaction) model;
            Categories = ft.Categories.Select(ftc => ftc.FinancialResourceCategory.Name).ToList();
            return Task.FromResult(new ViewModelMapResult());
        }
    }
}
