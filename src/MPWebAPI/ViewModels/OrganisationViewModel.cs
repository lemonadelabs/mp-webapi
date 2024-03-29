using System.ComponentModel.DataAnnotations;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public sealed class OrganisationViewModel : ViewModel
    {
        public OrganisationViewModel(Organisation org)
        {
            MapToViewModelAsync(org);
        }

        public OrganisationViewModel() {}

        public class Group
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class User
        {
            public string Name { get; set; }
            public string Id { get; set; }
        }

        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        public string ShortName { get; set; }
        
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }

        [EmailAddress]
        public string Contact { get; set; }

        public string WebDomain { get; set; }

        [Range(1, 12)]
        public int FinancialYearStart { get; set; }
    }
}