using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MPWebAPI.Filters;
using MPWebAPI.Models;
using MPWebAPI.Services;
using MPWebAPI.ViewModels;
using System.Net;

namespace MPWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserController : MerlinPlanController
    {
        // Post request models
        public class Register
        {
            public UserViewModel UserDetails { get; set; }
            public string Password { get; set; }
        }

        public class ConfirmEmailRequest
        {
            public string Code { get; set; }
            public string Password { get; set;}
            
            [EmailAddress]
            public string Email { get; set;}
        }

        public class PasswordResetRequest
        {
            [EmailAddress]
            public string Email { get; set; }
        }

        public class NewPasswordRequest
        {
            [EmailAddress]
            public string Email { get; set; }
            public string Code { get; set; }
            public string Password { get; set; }
        }

        private readonly IMerlinPlanBL _businessLogic;
        private readonly IMerlinPlanRepository _repository;
        private readonly UserManager<MerlinPlanUser> _userManager;
        private readonly IEmailSender _emailSender;

        public UserController(
            IMerlinPlanBL mpbl, 
            IMerlinPlanRepository repo, 
            UserManager<MerlinPlanUser> userManager,
            IEmailSender emailSender
            )
        {
            _businessLogic = mpbl;
            _repository = repo;
            _userManager = userManager;
            _emailSender = emailSender;
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
            var result = new List<UserViewModel>();
            var users = _userManager.Users.ToList();
            foreach (var u in users)
            {
                var uvm = new UserViewModel();
                await uvm.MapToViewModelAsync(u, _repository);
                result.Add(uvm);
            }
            return result;
        }

        [HttpGet("{id}")]
        [ValidateUserExists]
        public IActionResult Get(string id)
        {
            return new JsonResult(
                _repository.Users
                    .Where(u => u.Id == id)
                    .Select(async u => 
                    { 
                        var vm = new UserViewModel();
                        await vm.MapToViewModelAsync(u, _repository);
                        return vm; 
                    })
                    .Select(uvm => uvm.Result)
                    .Single()
                );
        }

        [HttpPost("password")]
        [ValidateModel]
        public async Task<IActionResult> Password([FromBody] NewPasswordRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Email);
            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(
                    user, 
                    request.Code, 
                    request.Password
                );
                
                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("passwordreset")]
        [ValidateModel]
        public async Task<IActionResult> PasswordReset([FromBody] PasswordResetRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Email);
            if (user != null)
            {
                if (await _userManager.IsEmailConfirmedAsync(user))
                {
                    var token = WebUtility.UrlEncode(await _userManager.GeneratePasswordResetTokenAsync(user));
                    var encodedEmail = WebUtility.UrlEncode(request.Email);
                    var callbackUrl = $"{_emailSender.UrlHost}/login/resetpassword?email={encodedEmail}&code={token}";
                    await _emailSender.SendEmailAsync(
                        user.UserName, 
                        "Reset your Merlin: Plan Password", 
                        $"Please reset your Merlin: Plan password by clicking this <a href=\"{callbackUrl}\">link</a>"
                    );
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("confirm")]
        [ValidateModel]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest emailConfirm)
        {
            var user = _repository.Users.Where(u => u.UserName == emailConfirm.Email).SingleOrDefault();

            if (user != null)
            {
                if (await _userManager.IsEmailConfirmedAsync(user))
                {
                    return BadRequest("User's email is already confirmed");                
                }
                
                var result = await _userManager.ConfirmEmailAsync(user, emailConfirm.Code);
                if (result.Succeeded)
                {
                    if (emailConfirm.Password != null)
                    {
                        // Change password
                        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                        var passwordChangeResult = await Password(new NewPasswordRequest 
                        {
                            Email = emailConfirm.Email,
                            Code = code,
                            Password = emailConfirm.Password
                        });
                        return passwordChangeResult;    
                    }
                    else
                    {
                        return Ok();
                    }
                    
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            else
            {
                return NotFound();
            }
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
                // Send email validation
                var code = WebUtility.UrlEncode(await _userManager.GenerateEmailConfirmationTokenAsync(user));
                var encodedUserName = WebUtility.UrlEncode(user.UserName);
                var callbackUrl = $"{_emailSender.UrlHost}/confirm/?email={encodedUserName}&code={code}";
                await _emailSender.SendEmailAsync(
                    user.UserName, 
                    "Confirm your Merlin: Plan account", 
                    $"Please confirm your Merlin: Plan account by clicking this <a href=\"{callbackUrl}\">link</a>"
                );

                var uvm = new UserViewModel();
                await uvm.MapToViewModelAsync(user, _repository);
                return new JsonResult(uvm);
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
                return BadRequest(result.Errors);
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
