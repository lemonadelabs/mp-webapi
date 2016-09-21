using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MPWebAPI.Models;
using MPWebAPI.ViewModels;

namespace MPWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        
        public class Register
        {
            public UserViewModel UserDetails { get; set; }
            public string Password { get; set; }
        }
        
        
        private UserManager<MerlinPlanUser> _userManager;
        private IMerlinPlanBL _mpbl;

        public UserController(UserManager<MerlinPlanUser> userManager, IMerlinPlanBL mpbl)
        {
            _userManager = userManager;
            _mpbl = mpbl;
        }

        [HttpGet]
        public IEnumerable<UserViewModel> Get()
        {
            return ConvertToViewModel(_userManager.Users.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var user = _userManager.Users.ToList().FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                return new JsonResult(ConvertToViewModel(new List<MerlinPlanUser> { user }).First());
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Register r)
        {
            if (ModelState.IsValid)
            {
                var user = new MerlinPlanUser();
                r.UserDetails.Id = user.Id;
                r.UserDetails.MapToModel(user);
                var result = await _mpbl.CreateUser(user, r.Password, r.UserDetails.Roles);
                if (result.Succeeded)
                {
                    return new JsonResult(ConvertToViewModel(new List<MerlinPlanUser> {user}).First());
                }
                else
                {
                    return new JsonResult(result);
                }                                                        
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UserViewModel user)
        {
            if(ModelState.IsValid)
            {
                var userm = _userManager.Users.ToList().FirstOrDefault(u => u.Id == id);
                if (userm != null)
                {
                    user.MapToModel(userm);
                    var result = await _userManager.UpdateAsync(userm);
                    if (result.Succeeded)
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest(result);
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            return BadRequest(ModelState);
        }
        

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = _userManager.Users.ToList().FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return new OkResult();
                }
                else
                {
                    return BadRequest(result);
                }
            }
            else
            {
                return NotFound();
            }
        }

        private IEnumerable<UserViewModel> ConvertToViewModel(IEnumerable<MerlinPlanUser> users)
        {
            var viewModels = new List<UserViewModel>();
            foreach (var u in users)
            {
                var uvm = new UserViewModel(u);
                uvm.Roles = _userManager.GetRolesAsync(u).Result;
                viewModels.Add(uvm);
            }
            return viewModels;
        }
    }
}
