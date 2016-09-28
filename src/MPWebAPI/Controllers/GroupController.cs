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
        private readonly IMerlinPlanRepository _repository;
        private readonly IMerlinPlanBL _businessLogic;
        private readonly UserManager<MerlinPlanUser> _userManager;

        public GroupController(
            IMerlinPlanRepository mprepo, 
            IMerlinPlanBL mpbl, 
            UserManager<MerlinPlanUser> userManager)
        {
            _repository = mprepo;
            _businessLogic = mpbl;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GroupViewModel group)
        {
            if (ModelState.IsValid)
            {
                var newGroup = new Group();
                group.MapToModel(newGroup);
                await _repository.AddGroupAsync(newGroup);
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
            return _repository.Groups.Select(g => new GroupViewModel(g));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var gr = _repository.Groups.First(g => g.Id == id);
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
            var gr = _repository.Groups.First(g => g.Id == id);
            if (gr != null)
            {
                var users = await _repository.GetGroupMembersAsync(gr);
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
                var group = _repository.Groups.First(g => g.Id == groupId);
                if (group != null)
                {
                    var userModels = await _userManager.Users.Where(u => r.Users.Contains(u.Id)).ToListAsync();
                    foreach (var u in userModels)
                    {
                        await _repository.AddUserToGroupAsync(u, group);    
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
                var group = _repository.Groups.First(g => g.Id == groupId);
                if (group != null)
                {
                    var userModels = await _userManager.Users.Where(u => r.Users.Contains(u.Id)).ToListAsync();
                    foreach (var u in userModels)
                    {
                        await _repository.RemoveUserFromGroupAsync(u, group);    
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
        

        [HttpDelete("{childId}/group")]
        public async Task<IActionResult> UnparentGroup(int childId)
        {
            var childGroup = _repository.Groups.FirstOrDefault(g => g.Id == childId);
            if (childGroup == null)
            {
                return NotFound();
            }
            var result = await _businessLogic.UnparentGroupAsync(childGroup);
            return Ok();
        }    


        [HttpPost("{childId}/group/{parentId}")]
        public async Task<IActionResult> ParentGroup(int childId, int parentId)
        {
            if (ModelState.IsValid)
            {
                var childGroup = _repository.Groups.FirstOrDefault(g => g.Id == childId);
                var parentGroup = _repository.Groups.FirstOrDefault(g => g.Id == parentId);

                if (childGroup == null || parentGroup == null)
                {
                    return NotFound();
                }
                var result = await _businessLogic.ParentGroupAsync(childGroup, parentGroup);
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
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var group = _repository.Groups.FirstOrDefault(g => g.Id == id);
            if (group != null)
            {
                await _repository.GroupSetActive(group, false);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
