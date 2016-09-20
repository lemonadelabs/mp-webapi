using System.Collections.Generic;
using System.Linq;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public class OrganisationViewModel : ViewModel
    {
        public OrganisationViewModel(Organisation org)
        {
            MapToViewModel(org);
            Groups = org.Groups.Select(g => new OrganisationViewModel.Group { Id = g.Id, Name = g.Name});
            Users = org.Users.Select(u => new OrganisationViewModel.User { Id = u.Id, Name = u.UserName});
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
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }
        public string Contact { get; set; }
        public string WebDomain { get; set; }
        public int FinancialYearStart { get; set; }
        public IEnumerable<OrganisationViewModel.Group> Groups { get; set; }
        public IEnumerable<OrganisationViewModel.User> Users { get; set; }
    }
}