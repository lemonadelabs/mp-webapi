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
    }
}
