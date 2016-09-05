using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MPWebAPI.Models;

namespace MPWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private UserManager<MerlinPlanUser> _userManager;
        private SignInManager<MerlinPlanUser> _signInManager;

        public UserController(
            UserManager<MerlinPlanUser> userManager, 
            SignInManager<MerlinPlanUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public class Register 
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
        
        [HttpGet]
        public IEnumerable<MerlinPlanUser> Get()
        {
            return _userManager.Users.ToList();
        }

        [HttpGet("{id}")]
        public MerlinPlanUser Get(string id)
        {
            return _userManager.Users.First(user => user.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Register r)
        {
            if (ModelState.IsValid)
            {
                var user = new MerlinPlanUser { UserName = r.Username, Email = r.Username };
                var result = await _userManager.CreateAsync(user, r.Password);
                if (result.Succeeded)
                {
                    return new JsonResult(user);
                }
                else
                {
                    return new JsonResult(result);
                }                                                        
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]MerlinPlanUser user)
        {
            _userManager.UpdateAsync(user);
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            var user = _userManager.Users.First(u => u.Id == id); 
            if (user != null)
            {
                _userManager.DeleteAsync(user);    
            }
        }
    }
}
