using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MPWebAPI.Models;
using MPWebAPI.ViewModels;

namespace MPWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class GroupController : MerlinPlanController
    {
        private readonly IMerlinPlanRepository _mprepo;
        private readonly IMerlinPlanBL _mpbl;
        private readonly UserManager<MerlinPlanUser> _userManager;

        public GroupController(
            IMerlinPlanRepository mprepo, 
            IMerlinPlanBL mpbl, 
            UserManager<MerlinPlanUser> userManager)
        {
            _mprepo = mprepo;
            _mpbl = mpbl;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GroupViewModel group)
        {
            if (ModelState.IsValid)
            {
                var newGroup = new Group();
                group.MapToModel(newGroup);
                await _mprepo.AddGroupAsync(newGroup);
                return new JsonResult(new GroupViewModel(newGroup));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        public IEnumerable<GroupViewModel> Get()
        {
            return _mprepo.Groups.Select(g => new GroupViewModel(g));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var gr = _mprepo.Groups.First(g => g.Id == id);
            if (gr != null)
            {
                return new JsonResult(new GroupViewModel(gr));
            }
            else
            {
                return NotFound();
            }

        }

        [HttpGet("{id}/user")]
        public async Task<IActionResult> GroupUser(int id)
        {
            var gr = _mprepo.Groups.First(g => g.Id == id);
            if (gr != null)
            {
                var users = await _mprepo.GetGroupMembersAsync(gr);
                var userViews = await ConvertToUserViewModelAsync(users, _userManager); 
                return new JsonResult(
                        userViews  
                    );
            }
            else
            {
                return NotFound();
            }
        }


        public class UserRequest
        {
            public IEnumerable<string> Users { get; set; }
        } 

        [HttpPost("{groupId}/user")]
        public async Task<IActionResult> AddUser(int groupId, [FromBody] UserRequest r)
        {
            if (ModelState.IsValid)
            {
                var group = _mprepo.Groups.First(g => g.Id == groupId);
                if (group != null)
                {
                    var userModels = await _userManager.Users.Where(u => r.Users.Contains(u.Id)).ToListAsync();
                    foreach (var u in userModels)
                    {
                        await _mprepo.AddUserToGroupAsync(u, group);    
                    }
                    return Ok();
                }
                else
                {
                    return NotFound();
                }    
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{groupId}/user")]
        public async Task<IActionResult> RemoveUser(int groupId, [FromBody] UserRequest r)
        {
            if (ModelState.IsValid)
            {
                var group = _mprepo.Groups.First(g => g.Id == groupId);
                if (group != null)
                {
                    var userModels = await _userManager.Users.Where(u => r.Users.Contains(u.Id)).ToListAsync();
                    foreach (var u in userModels)
                    {
                        await _mprepo.RemoveUserFromGroupAsync(u, group);    
                    }
                    return Ok();
                }
                else
                {
                    return NotFound();
                }         
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}
