using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        private readonly IMerlinPlanBL _businessLogic;
        private readonly IMerlinPlanRepository _repository;

        public UserController(IMerlinPlanBL mpbl, IMerlinPlanRepository repo)
        {
            _businessLogic = mpbl;
            _repository = repo;
        }
        
        [HttpGet("validate")]
        public IActionResult ValidateEmail([FromQuery] string email)
        {
            var result = _repository.Users.Any(u => u.Email == email);
            return new JsonResult(new {Valid = !result});
        }

        [HttpGet]
        public async Task<IEnumerable<UserViewModel>> Get()
        {
            return await ConvertToUserViewModelAsync(_repository.Users, _repository);
        }

        [HttpGet("{id}")]
        [ValidateUserExists]
        public IActionResult Get(string id)
        {
            return new JsonResult(
                ConvertToUserViewModelAsync(
                    new List<MerlinPlanUser> { _repository.Users.Single(u => u.Id == id) }, _repository).Result.First());
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
                return new JsonResult(ConvertToUserViewModelAsync(new List<MerlinPlanUser> {user}, _repository).Result.Single()) ;
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
            var userm = _repository.Users.Single(u => u.Id == id);
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
            var result = await _repository.UpdateUserAsync(userm);
            if (result.Succeeded)
            {
                
                // Update the user roles
                var currentRoles = await _repository.GetUserRolesAsync(userm);
                var rolesToDelete = currentRoles.Where(r => !user.Roles.Contains(r));
                var rolesToAdd = user.Roles.Where(r => !currentRoles.Contains(r));
                var roleRemoveResult = await _repository.RemoveUserFromRolesAsync(userm, rolesToDelete);

                if(roleRemoveResult.Succeeded)
                {
                    var roleAddResult = await _repository.AddUserToRolesAsync(userm, rolesToAdd);
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
            var user = _repository.Users.Single(u => u.Id == id);
            user.Active = false;
            await _repository.UpdateUserAsync(user);
            return Ok();
        }
    }
}
