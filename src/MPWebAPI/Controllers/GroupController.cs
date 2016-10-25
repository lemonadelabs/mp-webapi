using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public GroupController(IMerlinPlanRepository mprepo, IMerlinPlanBL mpbl)
        {
            _repository = mprepo;
            _businessLogic = mpbl;         
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Post([FromBody] GroupViewModel group)
        {
            // Check org is a valid organisation
            if (_repository.Organisations.All(o => o.Id != group.OrganisationId))
            {
                return BadRequest(new {OrganisationId = "Organisation does not exist."});
            }
            
            var newGroup = new Group();
            group.MapToModel(newGroup);
            await _repository.AddGroupAsync(newGroup);
            return new JsonResult(new GroupViewModel(newGroup));
        }

        [HttpGet]
        public List<GroupViewModel> Get()
        {
            return _repository.Groups.Select(g => new GroupViewModel(g)).ToList();
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
            var userViews = users
                .Select(async u => {
                    var vm = new UserViewModel();
                    await vm.MapToViewModelAsync(u, _repository);
                    return vm;
                })
                .Select(uvm => uvm.Result); 
            return new JsonResult(
                    userViews  
                );
        }

        public class UserRequest
        {
            public IEnumerable<string> Users { get; set; }
        } 

        [HttpPut("{id}/adduser")]
        [ValidateModel]
        [ValidateGroupExists]
        public async Task<IActionResult> AddUser(int id, [FromBody] UserRequest r)
        {
            var userModels = _repository.Users.Where(u => r.Users.Contains(u.Id));
            foreach (var u in userModels)
            {
                await _repository.AddUserToGroupAsync(u, _repository.Groups.Single(g => g.Id == id));    
            }
            return Ok();
        }

        [HttpPut("{id}/removeuser")]
        [ValidateModel]
        [ValidateGroupExists]
        public async Task<IActionResult> RemoveUser(int id, [FromBody] UserRequest r)
        {
            var userModels = _repository.Users.Where(u => r.Users.Contains(u.Id));
            foreach (var u in userModels)
            {
                await _repository.RemoveUserFromGroupAsync(u, _repository.Groups.Single(g => g.Id == id));    
            }
            return Ok();
        }
        

        [HttpPut("{id}/group")]
        [ValidateGroupExists]
        public async Task<IActionResult> UnparentGroup(int id)
        {
            var result = await _businessLogic.UnparentGroupAsync(_repository.Groups.Single(g => g.Id == id));
            if (result.Succeeded)
            {
                return Ok();    
            }
            else
            {
                return BadRequest(result.Errors);
            }
            
        }    


        [HttpPut("{childId}/group/{parentId}")]
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

        [HttpGet("{id}/category/financialresource")]
        [ValidateGroupExists]
        public IActionResult GetFinancialResourceCategories(int id)
        {
            return new JsonResult(_repository.FinancialResourceCategories
                .Where(frc => frc.GroupId == id)
                .Select(frc => new FinancialResourceCategoryViewModel(frc))
                .ToList());
        }

        [HttpPost("{id}/category/financialresource")]
        [ValidateGroupExists]
        [ValidateModel]
        public async Task<IActionResult> CreateFinancialResourceCategories(int id, [FromBody] FinancialResourceCategoryViewModel[] request)
        {
            var group = _repository.Groups.First(g => g.Id == id);
            var frcs = new List<FinancialResourceCategory>();
            foreach (var frc in request)
            {
                var newFRC = new FinancialResourceCategory();
                frc.MapToModel(newFRC);
                newFRC.GroupId = id;
                frcs.Add(newFRC);
            }

            var result = await _businessLogic.AddFinancialResourceCategoriesAsync(group, frcs);
            if (result.Succeeded)
            {
                return Ok(frcs.Select(frc => new FinancialResourceCategoryViewModel(frc)));
            }
            return BadRequest(result.Errors);
        }
    }
}
