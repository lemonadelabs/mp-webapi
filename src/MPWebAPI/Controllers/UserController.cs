using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MPWebAPI.Filters;
using MPWebAPI.Models;
using MPWebAPI.ViewModels;

namespace MPWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserController : MerlinPlanController
    {
        
        public class Register
        {
            public UserViewModel UserDetails { get; set; }
            public string Password { get; set; }
        }

        private UserManager<MerlinPlanUser> _userManager;
        private IMerlinPlanBL _businessLogic;

        public UserController(UserManager<MerlinPlanUser> userManager, IMerlinPlanBL mpbl)
        {
            _userManager = userManager;
            _businessLogic = mpbl;
        }
        
        [HttpGet("validate")]
        public IActionResult ValidateEmail([FromQuery] string email)
        {
            var result = _userManager.Users.Any(u => u.Email == email);
            return new JsonResult(new {Valid = !result});
        }

        [HttpGet]
        public IEnumerable<UserViewModel> Get()
        {
            return ConvertToUserViewModelAsync(_userManager.Users.ToList(), _userManager).Result;
        }

        [HttpGet("{id}")]
        [ValidateUserExists]
        public IActionResult Get(string id)
        {
            return new JsonResult(
                ConvertToUserViewModelAsync(
                    new List<MerlinPlanUser> { _userManager.Users.Single(u => u.Id == id) }, _userManager).Result.First());
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Post([FromBody] Register r)
        {
            var user = new MerlinPlanUser();
            r.UserDetails.Id = user.Id;
            r.UserDetails.MapToModel(user);
            var result = await _businessLogic.CreateUser(user, r.Password, r.UserDetails.Roles);
            if (result.Succeeded)
            {
                return new JsonResult(ConvertToUserViewModelAsync(new List<MerlinPlanUser> {user}, _userManager).Result.Single()) ;
            }
            else
            {
                return new JsonResult(result);
            }                                                        
        }

        [HttpPut("{id}")]
        [ValidateModel]
        [ValidateUserExists]
        public async Task<IActionResult> Put(string id, [FromBody] UserViewModel user)
        {
            var userm = _userManager.Users.Single(u => u.Id == id);
            if (user.Id != id)
            {
                return BadRequest(
                    new {
                            Id = new List<string> {"User id in the url request must match the Id in the JSON body"}
                        }
                    );
            }
            
            // Update the user details
            user.MapToModel(userm);
            var result = await _userManager.UpdateAsync(userm);
            if (result.Succeeded)
            {
                
                // Update the user roles
                var currentRoles = await _userManager.GetRolesAsync(userm);
                var rolesToDelete = currentRoles.Where(r => !user.Roles.Contains(r));
                var rolesToAdd = user.Roles.Where(r => !currentRoles.Contains(r));
                var roleRemoveResult = await _userManager.RemoveFromRolesAsync(userm, rolesToDelete);

                if(roleRemoveResult.Succeeded)
                {
                    var roleAddResult = await _userManager.AddToRolesAsync(userm, rolesToAdd);
                    if (roleAddResult.Succeeded)
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest(roleAddResult);
                    }
                }
                else
                {
                    return BadRequest(roleRemoveResult);
                } 

            }
            else
            {
                return BadRequest(result);
            }
        }
        

        [HttpDelete("{id}")]
        [ValidateUserExists]
        public async Task<IActionResult> Delete(string id)
        {
            var user = _userManager.Users.Single(u => u.Id == id);
            user.Active = false;
            await _userManager.UpdateAsync(user);
            return Ok();
        }
    }
}
