using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MPWebAPI.Models;
using MPWebAPI.ViewModels;

namespace MPWebAPI.Controllers
{
    /// <summary>
    /// Base class for all MP Controllers. Contains common functionality
    /// </summary>
    public class MerlinPlanController : Controller
    {
        protected async Task<IEnumerable<UserViewModel>> ConvertToUserViewModelAsync (
            IEnumerable<MerlinPlanUser> users, 
            IMerlinPlanRepository repo
            )
        {
            var viewModels = new List<UserViewModel>();
            foreach (var u in users)
            {
                var uvm = new UserViewModel(u);
                uvm.Roles = await repo.GetUserRolesAsync(u);
                var gs = await repo.GetUserGroupsAsync(u);
                uvm.Groups = gs.Select(g => new UserViewModel.GroupData { Id = g.Id, Name = g.Name});
                viewModels.Add(uvm);
            }
            return viewModels;
        }
    }
}