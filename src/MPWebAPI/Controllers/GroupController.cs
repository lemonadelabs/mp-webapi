using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MPWebAPI.Models;
using MPWebAPI.ViewModels;
using MPWebAPI.Filters;

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
        [ValidateModel]
        public async Task<IActionResult> Post([FromBody] GroupViewModel group)
        {
            var newGroup = new Group();
            group.MapToModel(newGroup);
            await _repository.AddGroupAsync(newGroup);
            return new JsonResult(new GroupViewModel(newGroup));
        }

        [HttpGet]
        public IEnumerable<GroupViewModel> Get()
        {
            return _repository.Groups.Select(g => new GroupViewModel(g));
        }

        [HttpGet("{id}")]
        [ValidateGroupExists]
        public IActionResult Get(int id)
        {
            return new JsonResult(new GroupViewModel(_repository.Groups.Single(g => g.Id == id)));
        }

        [HttpGet("{id}/user")]
        [ValidateGroupExists]
        public async Task<IActionResult> GroupUser(int id)
        {
            var users = await _repository.GetGroupMembersAsync(_repository.Groups.Single(g => g.Id == id));
            var userViews = await ConvertToUserViewModelAsync(users, _userManager); 
            return new JsonResult(
                    userViews  
                );
        }

        public class UserRequest
        {
            public IEnumerable<string> Users { get; set; }
        } 

        [HttpPost("{id}/user")]
        [ValidateModel]
        [ValidateGroupExists]
        public async Task<IActionResult> AddUser(int id, [FromBody] UserRequest r)
        {
            var userModels = await _userManager.Users.Where(u => r.Users.Contains(u.Id)).ToListAsync();
            foreach (var u in userModels)
            {
                await _repository.AddUserToGroupAsync(u, _repository.Groups.Single(g => g.Id == id));    
            }
            return Ok();
        }

        [HttpDelete("{id}/user")]
        [ValidateModel]
        [ValidateGroupExists]
        public async Task<IActionResult> RemoveUser(int id, [FromBody] UserRequest r)
        {
            var userModels = await _userManager.Users.Where(u => r.Users.Contains(u.Id)).ToListAsync();
            foreach (var u in userModels)
            {
                await _repository.RemoveUserFromGroupAsync(u, _repository.Groups.Single(g => g.Id == id));    
            }
            return Ok();
        }
        

        [HttpDelete("{id}/group")]
        [ValidateGroupExists]
        public async Task<IActionResult> UnparentGroup(int id)
        {
            var result = await _businessLogic.UnparentGroupAsync(_repository.Groups.Single(g => g.Id == id));
            return Ok();
        }    


        [HttpPost("{childId}/group/{parentId}")]
        [ValidateModel]
        public async Task<IActionResult> ParentGroup(int childId, int parentId)
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

        [HttpDelete("{id}")]
        [ValidateGroupExists]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.GroupSetActive(_repository.Groups.Single(g => g.Id == id), false);
            return Ok();
        }
    }
}
