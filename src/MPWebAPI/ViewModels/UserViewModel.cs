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
            UserEmailConfirmed = u.EmailConfirmed;
        }

        public UserViewModel() 
        {
            Active = true;
        }

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
        
        [Required]
        public string LastName { get; set; }
        public string NickName { get; set; }

        public IEnumerable<string> Roles { get; set; }
        public IEnumerable<GroupData> Groups { get; set; }

        public bool UserEmailConfirmed { get; set; }

        public class GroupData
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }

}