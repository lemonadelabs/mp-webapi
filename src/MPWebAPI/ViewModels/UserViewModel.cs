using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{

    public class UserViewModel : ViewModel
    {
        public UserViewModel() 
        {
            Active = true;
        }

        public async override Task MapToViewModelAsync(object user, IMerlinPlanRepository repo)
        {
            var u = (MerlinPlanUser)user;
            await base.MapToViewModelAsync(u);
            UserEmailConfirmed = u.EmailConfirmed;
            this.Roles = await repo.GetUserRolesAsync(u);
            var gs = await repo.GetUserGroupsAsync(u);
            this.Groups = gs.Select(g => new UserViewModel.GroupData { Id = g.Id, Name = g.Name});
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