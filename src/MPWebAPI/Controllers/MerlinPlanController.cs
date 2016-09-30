using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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
            UserManager<MerlinPlanUser> userManager
            )
        {
            var viewModels = new List<UserViewModel>();
            foreach (var u in users)
            {
                var uvm = new UserViewModel(u);
                uvm.Roles = await userManager.GetRolesAsync(u);
                viewModels.Add(uvm);
            }
            return viewModels;
        }
    }
}