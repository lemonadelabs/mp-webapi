using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{

    public class UserViewModel : ViewModel
    {
        public UserViewModel(MerlinPlanUser u)
        {
            MapToViewModel(u);
        }

        public UserViewModel() {}

        public string Id { get; set; }

        public bool Active { get; set; }

        [EmailAddress]
        [Required]
        public string UserName { get; set; }

        [Required]
        public int OrganisationId { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }
        public string EmployeeId { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string NickName { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }

}