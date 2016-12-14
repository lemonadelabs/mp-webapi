using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MPWebAPI.Models;
using MPWebAPI.ViewModels;
using MPWebAPI.Filters;


namespace MPWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class ResourceScenarioController : ControllerBase
    {
        public class UserList
        {
            public IEnumerable<string> Users { get; set; }
        }

        public class ResourceScenarioCopyRequest : IDocumentCopyRequest
        {
            [Required]
            public int Id { get; set; }

            [Required]
            public int Group { get; set; }

            public string Name { get; set; }

            [Required]
            public string User { get; set; }
        }

        public sealed class NewFinancialResourceRequest : ViewModel
        {
            public int ResourceScenarioId { get; set; }

            [Required]
            public string Name { get; set; }

            [Required]
            public DateTime StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public bool Recurring { get; set; }
            public decimal? DefaultPartitionValue { get; set; }

            public NewFinancialResourceRequest(FinancialResource model)
            {
                MapToViewModelAsync(model);
            }

            public NewFinancialResourceRequest()
            {
            }
        }
        
        private readonly IMerlinPlanRepository _repository;
        private readonly IMerlinPlanBL _businessLogic;

        public ResourceScenarioController(
            IMerlinPlanRepository repository, 
            IMerlinPlanBL businessLogic
            )
        {
            _repository = repository;
            _businessLogic = businessLogic;
        }
        
        [HttpGet]
        public IEnumerable<ResourceScenarioViewModel> GetAll()
        {
            return _repository.ResourceScenarios.Select(
                rs => new ResourceScenarioViewModel(rs));
        }

        [HttpGet("{id}")]
        [ValidateResourceScenarioExists]
        public IActionResult Get(int id)
        {
            return Ok(new ResourceScenarioViewModel(_repository.ResourceScenarios.Single(rs => rs.Id == id)));
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
            var newRs = new ResourceScenario();
            
            // Do basic mapping
            await scenario.MapToModel(newRs, _repository);

            // Add creator object
            // check creator is valid user            
            var creator = await _repository.FindUserByUserNameAsync(scenario.Creator);
            if (creator == null)
            {
                return BadRequest(new {Creator = $"Creator {scenario.Creator} can't be found"});
            }
            newRs.Creator = creator;

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
                return BadRequest(new { Group = "Creator doesnt belong to group"});
            }
            newRs.Group = group;

            // Add approved by
            // check that user is valid
            if (scenario.ApprovedBy != null)
            {
                var approvedBy = await _repository.FindUserByUserNameAsync(scenario.ApprovedBy);
                if (approvedBy == null)
                {
                    return BadRequest(new { ApprovedBy = $"Approver {scenario.ApprovedBy} can't be found" });
                }
                newRs.ApprovedBy = approvedBy;    
            }

            await _repository.AddResourceScenarioAsync(newRs);
            return new JsonResult(new ResourceScenarioViewModel(newRs));
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
            var rs = _repository.ResourceScenarios.Single(r => r.Id == id);
            await _repository.ShareResourceScenarioWithGroupAsync(rs, true);
            return Ok(new ResourceScenarioViewModel(rs));
        }

        [HttpPut("{id}/group/unshare")]
        [ValidateResourceScenarioExists]
        public async Task<IActionResult> UnshareWithGroup(int id)
        {
            var rs = _repository.ResourceScenarios.Single(r => r.Id == id);
            await _repository.ShareResourceScenarioWithGroupAsync(rs, false);
            return Ok(new ResourceScenarioViewModel(rs));
        }

        [HttpPut("{id}/share")]
        [ValidateResourceScenarioExists]
        public async Task<IActionResult> ShareWithOrg(int id)
        {
            var rs = _repository.ResourceScenarios.Single(r => r.Id == id);
            await _repository.ShareResourceScenarioWithOrgAsync(rs, true);
            return Ok(new ResourceScenarioViewModel(rs));
        }

        [HttpPut("{id}/unshare")]
        [ValidateResourceScenarioExists]
        public async Task<IActionResult> UnshareWithOrg(int id)
        {
            var rs = _repository.ResourceScenarios.Single(r => r.Id == id);
            await _repository.ShareResourceScenarioWithOrgAsync(rs, false);
            return Ok(new ResourceScenarioViewModel(rs));
        }

        [HttpPut("{id}/user/share")]
        [ValidateResourceScenarioExists]
        public async Task<IActionResult> ShareWithUser(int id, [FromBody] UserList userNameList)
        {
            var rs = _repository.ResourceScenarios.Single(r => r.Id == id);
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
            var rs = _repository.ResourceScenarios.Single(r => r.Id == id);
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
            var user = _repository.Users.Single(u => u.Id == id);
            var documents = new List<ResourceScenario>();

            var userShare = await _repository.GetUserSharedResourceScenariosForUserAsync(user);
            var groupShare = await _repository.GetGroupSharedResourceScenariosForUserAsync(user);
            var allShare = await _repository.GetOrganisationSharedResourceScenariosAsync(user.Organisation);
            var owned = _repository.ResourceScenarios.Where(rs => rs.Creator.Id == id);

            documents.AddRange(userShare.Where(d => !documents.Select(did => did.Id).Contains(d.Id)));
            documents.AddRange(groupShare.Where(d => !documents.Select(did => did.Id).Contains(d.Id)));
            documents.AddRange(allShare.Where(d => !documents.Select(did => did.Id).Contains(d.Id)));
            documents.AddRange(owned.Where(d => !documents.Select(did => did.Id).Contains(d.Id)));

            var groups = new List<Group>();
            foreach (var document in documents)
            {
                if (!groups.Select(g => g.Id).Contains(document.Group.Id))
                {
                    groups.Add(document.Group);
                }
            }

            return Ok(
                new AccessibleDocumentViewModel<ResourceScenarioViewModel>
                {
                    Documents = documents.Select(d => new ResourceScenarioViewModel(d)).ToList(),
                    Groups = groups.Select(g => new GroupViewModel(g)).ToList()
                }
            );
        }


        [HttpPost("{id}/financialresource")]
        [ValidateResourceScenarioExists]
        [ValidateModel]
        public async Task<IActionResult> AddFinancialResource(int id, [FromBody] NewFinancialResourceRequest viewModel)
        {
            var fr = new FinancialResource();
            viewModel.ResourceScenarioId = id;
            await viewModel.MapToModel(fr);
            
            var result = await _businessLogic.AddFinancialResourceAsync(fr, viewModel.DefaultPartitionValue);
            
            if (result.Succeeded)
            {
                return Ok(new FinancialResourceViewModel(fr));    
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("{id}/staffresource")]
        [ValidateResourceScenarioExists]
        [ValidateModel]
        public async Task<IActionResult> AddStaffResource(int id, [FromBody] StaffResourceViewModel viewModel)
        {
            var sr = new StaffResource();
            viewModel.ResourceScenarioId = id;
            await viewModel.MapToModel(sr);
            await _repository.AddStaffResourceAsync(sr);
            return Ok(new StaffResourceViewModel(sr));
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
                .Select(async sr => 
                {
                    var srvm = new StaffResourceViewModel();
                    await srvm.MapToViewModelAsync(sr, _repository);
                    return srvm;
                }).Select(srvm => srvm.Result));
        }

        [HttpGet("{id}/resources")]
        [ValidateResourceScenarioExists]
        public IActionResult GetAllResources(int id)
        {
            var staffResources = _repository.StaffResources
                .Where(sr => sr.ResourceScenarioId == id)
                .Select(async sr => 
                {
                    var srvm = new StaffResourceViewModel();
                    await srvm.MapToViewModelAsync(sr, _repository);
                    return srvm;
                }).Select(srvm => srvm.Result);

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

        [HttpPost("copy")]
        [ValidateModel]
        public async Task<IActionResult> CopyScenario([FromBody] ResourceScenarioCopyRequest[] requests)
        {
            var result = await _businessLogic.CopyResourceScenariosAsync(requests);
            if (result.Succeeded)
            {
                return
                    Ok(result
                        .GetData<IEnumerable<ResourceScenario>>()
                        .Select(rs => new ResourceScenarioViewModel(rs))
                       );
            }
            return BadRequest(result.Errors);
        }
    }
}
