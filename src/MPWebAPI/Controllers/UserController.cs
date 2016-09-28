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

        [HttpGet]
        public IEnumerable<UserViewModel> Get()
        {
            return ConvertToUserViewModelAsync(_userManager.Users.ToList(), _userManager).Result;
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var user = _userManager.Users.ToList().FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                return new JsonResult(ConvertToUserViewModelAsync(new List<MerlinPlanUser> { user }, _userManager).Result.First());
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
                user.Active = false;
                await _userManager.UpdateAsync(user);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
