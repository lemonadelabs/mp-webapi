using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MPWebAPI.Models;
using MPWebAPI.ViewModels;
using MPWebAPI.Filters;
using UserSharedScenario = MPWebAPI.Controllers.ResourceScenarioController.UserAccessResponse.UserSharedScenario;
using GroupSharedScenario = MPWebAPI.Controllers.ResourceScenarioController.UserAccessResponse.GroupSharedScenario;
using Microsoft.AspNetCore.Authorization;

namespace MPWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class ResourceScenarioController : Controller
    {
        public class UserAccessResponse
        {
            public class UserSharedScenario
            {
                public UserViewModel User { get; set; }
                public List<ResourceScenarioViewModel> Scenarios { get; set; }
            }

            public class GroupSharedScenario
            {
                public GroupViewModel Group { get; set; }
                public List<ResourceScenarioViewModel> Scenarios { get; set; }
            }
            
            public List<ResourceScenarioViewModel> Created { get; set; }
            public List<GroupSharedScenario> GroupShare { get; set; }
            public List<UserSharedScenario> UserShare { get; set; }
            public List<GroupSharedScenario> OrgShare { get; set; }
        }

        public class UserList
        {
            public IEnumerable<string> Users { get; set; }
        }
        
        private readonly IMerlinPlanRepository _repository;
        private readonly IMerlinPlanBL _businessLogic;
        private readonly ILogger _logger;
        
        public ResourceScenarioController(
            IMerlinPlanRepository repository, 
            IMerlinPlanBL businessLogic,
            ILoggerFactory loggerFactory
            )
        {
            _repository = repository;
            _businessLogic = businessLogic;
            _logger = loggerFactory.CreateLogger<ResourceScenarioController>();
        }
        
        [HttpGet]
        public IEnumerable<ResourceScenarioViewModel> Get()
        {
            return _repository.ResourceScenarios.Select(
                rs => new ResourceScenarioViewModel(rs));
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Project Admin, Lemonade Admin")]
        [ValidateResourceScenarioExists]
        public async Task<IActionResult> Delete(int id)
        {
            var scenario = _repository.ResourceScenarios.First(rs => rs.Id == id);
            await _repository.RemoveResourceScenarioAsync(scenario);
            return Ok();
        }

        [HttpPost]
        //[Authorize(Roles = "Project Admin, Lemonade Admin")]
        [ValidateModel]
        public async Task<IActionResult> Post([FromBody] ResourceScenarioViewModel scenario)
        {
            var newRS = new ResourceScenario();
            
            // Do basic mapping
            scenario.MapToModel(newRS);

            // Add creator object
            // check creator is valid user            
            var creator = await _repository.FindUserByUserNameAsync(scenario.Creator);
            if (creator == null)
            {
                return BadRequest(new {Creator = $"Creator {scenario.Creator} can't be found"});
            }
            newRS.Creator = creator;

            // Add group object
            // check that group is valid
            var group = _repository.Groups.FirstOrDefault(g => g.Id == scenario.Group);
            if (group == null)
            {
                return BadRequest(new {Group = $"Group with id {scenario.Group} not found"});
            }
            
            // check that creator belongs to group
            var gMembers = await _repository.GetGroupMembersAsync(group);
            if (!gMembers.Contains(creator))
            {
                return BadRequest(new { Group = $"Creator doesnt belong to group"});
            }
            newRS.Group = group;

            // Add approved by
            // check that user is valid
            if (scenario.ApprovedBy != null)
            {
                var approvedBy = await _repository.FindUserByUserNameAsync(scenario.ApprovedBy);
                if (approvedBy == null)
                {
                    return BadRequest(new { ApprovedBy = $"Approver {scenario.ApprovedBy} can't be found" });
                }
                newRS.ApprovedBy = approvedBy;    
            }

            await _repository.AddResourceScenarioAsync(newRS);
            return new JsonResult(new ResourceScenarioViewModel(newRS));
        }

        [HttpGet("group/{id}")]
        [ValidateGroupExists]
        public IActionResult GetForGroup(int id)
        {
            return new JsonResult(
                _repository.ResourceScenarios
                    .Where(rs => rs.Group.Id == id)
                    .Select(rs => new ResourceScenarioViewModel(rs))
            );
        }

        [HttpGet("user/{id}")]
        [ValidateUserExists]
        public IActionResult GetCreatedByUser(string id)
        {
            return new JsonResult(
                _repository.ResourceScenarios
                    .Where(rs => rs.Creator.Id == id)
                    .Select(rs => new ResourceScenarioViewModel(rs))
            );
        }

        [HttpPut("{id}/group/share")]
        [ValidateResourceScenarioExists]
        public async Task<IActionResult> ShareWithGroup(int id)
        {
            var rs = _repository.ResourceScenarios.Where(r => r.Id == id).Single();
            await _repository.ShareResourceScenarioWithGroupAsync(rs, true);
            return Ok();
        }

        [HttpPut("{id}/group/unshare")]
        [ValidateResourceScenarioExists]
        public async Task<IActionResult> UnshareWithGroup(int id)
        {
            var rs = _repository.ResourceScenarios.Where(r => r.Id == id).Single();
            await _repository.ShareResourceScenarioWithGroupAsync(rs, false);
            return Ok();
        }

        [HttpPut("{id}/share")]
        [ValidateResourceScenarioExists]
        public async Task<IActionResult> ShareWithOrg(int id)
        {
            var rs = _repository.ResourceScenarios.Where(r => r.Id == id).Single();
            await _repository.ShareResourceScenarioWithOrgAsync(rs, true);
            return Ok();
        }

        [HttpPut("{id}/unshare")]
        [ValidateResourceScenarioExists]
        public async Task<IActionResult> UnshareWithOrg(int id)
        {
            var rs = _repository.ResourceScenarios.Where(r => r.Id == id).Single();
            await _repository.ShareResourceScenarioWithOrgAsync(rs, false);
            return Ok();
        }

        [HttpPut("{id}/user/share")]
        [ValidateResourceScenarioExists]
        public async Task<IActionResult> ShareWithUser(int id, [FromBody] UserList userNameList)
        {
            var rs = _repository.ResourceScenarios.Where(r => r.Id == id).Single();
            var users = new List<MerlinPlanUser>();
            foreach (var userName in userNameList.Users)
            {
               var u = await _repository.FindUserByUserNameAsync(userName);
               if (u != null)
               {
                   users.Add(u);
               }
               else
               {
                   return BadRequest(new { Users = $"User {userName} does not exist." });
               } 
            }

            foreach (var user in users)
            {
                await _repository.ShareResourceScenarioWithUserAsync(rs, user);
            }
            return Ok();
        }

        [HttpPut("{id}/user/unshare")]
        [ValidateResourceScenarioExists]
        public async Task<IActionResult> UnshareWithUser(int id, [FromBody] UserList userNameList)
        {
            var rs = _repository.ResourceScenarios.Where(r => r.Id == id).Single();
            var users = new List<MerlinPlanUser>();
            foreach (var userName in userNameList.Users)
            {
               var u = await _repository.FindUserByUserNameAsync(userName);
               if (u != null)
               {
                   users.Add(u);
               }
               else
               {
                   return BadRequest(new { Users = $"User {userName} does not exist." });
               } 
            }

            foreach (var user in users)
            {
                await _repository.UnshareResourceScenarioWithUserAsync(rs, user);
            }
            return Ok();
        }

        [HttpGet("useraccess/{id}")]
        [ValidateUserExists]
        public async Task<IActionResult> GetAllForUser(string id)
        {
            var user = _repository.Users.Where(u => u.Id == id).Single();
            var userShare = await _repository.GetUserSharedResourceScenariosForUserAsync(user);
            var groupShare = await _repository.GetGroupSharedResourceScenariosForUserAsync(user);
            var allShare = await _repository.GetOrganisationSharedResourceScenariosAsync(user.Organisation);
            var owned = _repository.ResourceScenarios.Where(rs => rs.Creator.Id == id);

            var groupSharedScenarios = ResourceScenariosByGroup(groupShare.ToList());
            var orgSharedScenarios = ResourceScenariosByGroup(allShare.ToList());

            var userSharedScenarios = new List<UserSharedScenario>();
            foreach (var rs in userShare)
            {
                var u = userSharedScenarios.FirstOrDefault(uss => uss.User.UserName == rs.Creator.UserName);
                if (u != null)
                {
                    u.Scenarios.Add(new ResourceScenarioViewModel(rs));                    
                }
                else
                {
                    var newuss = new UserSharedScenario
                    {
                        User = new UserViewModel(rs.Creator),
                        Scenarios = new List<ResourceScenarioViewModel>(
                            new ResourceScenarioViewModel[] { new ResourceScenarioViewModel(rs) })
                    };
                    userSharedScenarios.Add(newuss);
                }
            }

            return new JsonResult(
                new UserAccessResponse 
                {
                    Created = owned.Select(o => new ResourceScenarioViewModel(o)).ToList(),
                    GroupShare = groupSharedScenarios,
                    OrgShare = orgSharedScenarios,
                    UserShare = userSharedScenarios
                }
            );
        }

        [HttpPost("{id}/financialresource")]
        [ValidateResourceScenarioExists]
        [ValidateModel]
        public async Task<IActionResult> AddFinancialResource(int id, [FromBody] FinancialResourceViewModel viewModel)
        {
            var fr = new FinancialResource();
            viewModel.ResourceScenarioId = id;
            viewModel.MapToModel(fr);

            await _repository.AddFinancialResourceAsync(fr);
            
            return Ok();
        }

        [HttpGet("{id}/financialresource")]
        [ValidateResourceScenarioExists]
        public IActionResult GetFinancialResources(int id)
        {
            return new JsonResult(_repository.FinancialResources
                .Where(fr => fr.ResourceScenarioId == id)
                .Select(fr => new FinancialResourceViewModel(fr)));
        }

        [HttpGet("{id}/staffresource")]
        [ValidateResourceScenarioExists]
        public IActionResult GetStaffResources(int id)
        {
            return new JsonResult(_repository.StaffResources
                .Where(sr => sr.ResourceScenarioId == id)
                .Select(sr => new StaffResourceViewModel(sr)));
        }

        [HttpGet("{id}/resources")]
        [ValidateResourceScenarioExists]
        public IActionResult GetAllResources(int id)
        {
            var staffResources = _repository.StaffResources
                .Where(sr => sr.ResourceScenarioId == id)
                .Select(sr => new StaffResourceViewModel(sr));

            var financialResources = _repository.FinancialResources
                .Where(fr => fr.ResourceScenarioId == id)
                .Select(fr => new FinancialResourceViewModel(fr));
            
            return new JsonResult(
                new { 
                    StaffResources = staffResources, 
                    FinancialResources = financialResources 
                    }
                );
        }

        private List<GroupSharedScenario> ResourceScenariosByGroup(List<ResourceScenario> scenarios)
        {
            var groupedScenarios = new List<GroupSharedScenario>();
            foreach (var rs in scenarios)
            {
                var g = groupedScenarios.FirstOrDefault(ass => ass.Group.Id == rs.Group.Id);
                if (g != null)
                {
                    g.Scenarios.Add(new ResourceScenarioViewModel(rs));
                }
                else
                {
                    var newass = new GroupSharedScenario 
                    {
                        Group = new GroupViewModel(rs.Group),
                        Scenarios = new List<ResourceScenarioViewModel>(
                            new ResourceScenarioViewModel[] {new ResourceScenarioViewModel(rs)})
                    };
                    groupedScenarios.Add(newass);
                }
            }
            return groupedScenarios;
        }
    }
}
