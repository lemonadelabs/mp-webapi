using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MPWebAPI.Filters;
using MPWebAPI.Models;
using MPWebAPI.ViewModels;
using UserShared = MPWebAPI.ViewModels.AccessibleDocumentViewModel<MPWebAPI.ViewModels.ProjectViewModel>.UserShared;


namespace MPWebAPI.Controllers
{
    [Route("api/[Controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IMerlinPlanBL _businesLogic;
        private readonly IMerlinPlanRepository _repository;

        public class UserList
        {
            public IEnumerable<string> Users { get; set; }
        }

        public class ProjectCopyRequest : IDocumentCopyRequest
        {
            public int Id { get; set; }
            public int Group { get; set; }
            public string Name { get; set; }
            public string User { get; set; }
        }

        public ProjectController(IMerlinPlanRepository repo, IMerlinPlanBL mpbl)
        {
            _businesLogic = mpbl;
            _repository = repo;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_repository.Projects.Select(p => new ProjectViewModel(p)));

        [HttpGet("{id}")]
        [ValidateProjectExists]
        public IActionResult Get(int id) => Ok(new ProjectViewModel(_repository.Projects.Single(p => p.Id == id)));

        [HttpGet("group/{id}")]
        [ValidateGroupExists]
        public IActionResult GetAllForGroup(int id)
        {
            return Ok(_repository.Projects.Where(p => p.Group.Id == id).Select(p => new ProjectViewModel(p)));
        }

        [HttpGet("user/{id}")]
        [ValidateUserExists]
        public IActionResult GetUserCreated(string id)
        {
            return Ok(_repository.Projects.Where(p => p.Creator.Id == id).Select(p => new ProjectViewModel(p)));
        }

        [HttpGet("useraccess/{id}")]
        [ValidateUserExists]
        public async Task<IActionResult> GetAllForUser(string id)
        {
            var user = _repository.Users.Single(u => u.Id == id);
            var userShare = _repository.GetUserSharedProjectsForUserAsync(user);
            var groupShare = _repository.GetGroupShareProjectsForUserAsync(user);
            var allShare = _repository.GetOrganisationSharedProjectsAsync(user.Organisation);
            var owned = _repository.Projects.Where(rs => rs.Creator.Id == id);

            var groupSharedProjects = AccessibleDocumentViewModel<ProjectViewModel>.DocumentsByGroup(groupShare.ToList());
            var orgSharedProjects = AccessibleDocumentViewModel<ProjectViewModel>.DocumentsByGroup(allShare.ToList());

            var userSharedProjects = new List<UserShared>();
            foreach (var rs in userShare)
            {
                var u = userSharedProjects.FirstOrDefault(uss => uss.User.UserName == rs.Creator.UserName);
                if (u != null)
                {
                    u.Documents.Add(new ProjectViewModel(rs));
                }
                else
                {
                    var uvm = new UserViewModel();
                    await uvm.MapToViewModelAsync(rs.Creator, _repository);
                    var newuss = new UserShared
                    {
                        User = uvm,
                        Documents = new List<ProjectViewModel>(
                            new[] {new ProjectViewModel(rs)})
                    };
                    userSharedProjects.Add(newuss);
                }
            }

            return new JsonResult(
                new AccessibleDocumentViewModel<ProjectViewModel>()
                {
                    Created = owned.Select(o => new ProjectViewModel(o)).ToList(),
                    GroupShare = groupSharedProjects,
                    OrgShare = orgSharedProjects,
                    UserShare = userSharedProjects
                }
            );
        }

        [HttpPut("{id}/group/share")]
        [ValidateProjectExists]
        public async Task<IActionResult> GroupShare(int id)
        {
            var project = _repository.Projects.Single(p => p.Id == id);
            await _repository.ShareProjectWithGroupAsync(project, true);
            return Ok(new ProjectViewModel(project));
        }

        [HttpPut("{id}/group/unshare")]
        [ValidateProjectExists]
        public async Task<IActionResult> GroupUnshare(int id)
        {
            var project = _repository.Projects.Single(p => p.Id == id);
            await _repository.ShareProjectWithGroupAsync(project, false);
            return Ok(new ProjectViewModel(project));
        }

        [HttpPut("{id}/share")]
        [ValidateProjectExists]
        public async Task<IActionResult> Share(int id)
        {
            var project = _repository.Projects.Single(p => p.Id == id);
            await _repository.ShareProjectWithOrgAsync(project, true);
            return Ok(new ProjectViewModel(project));
        }

        [HttpPut("{id}/unshare")]
        [ValidateProjectExists]
        public async Task<IActionResult> Unshare(int id)
        {
            var project = _repository.Projects.Single(p => p.Id == id);
            await _repository.ShareProjectWithOrgAsync(project, false);
            return Ok(new ProjectViewModel(project));
        }


        [HttpPut("{id}/user/share")]
        [ValidateProjectExists]
        public async Task<IActionResult> ShareWithUser(int id, [FromBody] UserList userNameList)
        {
            var ps = _repository.Projects.Single(r => r.Id == id);
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
                    return BadRequest(new {Users = $"User {userName} does not exist."});
                }
            }

            foreach (var user in users)
            {
                await _repository.ShareProjectWithUserAsync(ps, user);
            }
            return Ok();
        }

        [HttpPut("{id}/user/unshare")]
        [ValidateProjectExists]
        public async Task<IActionResult> UnshareWithUser(int id, [FromBody] UserList userNameList)
        {
            var ps = _repository.Projects.Single(r => r.Id == id);
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
                    return BadRequest(new {Users = $"User {userName} does not exist."});
                }
            }

            foreach (var user in users)
            {
                await _repository.UnshareProjectWithUserAsync(ps, user);
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        [ValidateProjectExists]
        public async Task<IActionResult> Delete(int id)
        {
            var project = _repository.Projects.Single(p => p.Id == id);
            var result = await _businesLogic.DeleteProjectAsync(project);
            if (result.Succeeded)
            {
                return Ok(id);
            }
            return BadRequest(result.Errors);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateProject([FromBody] ProjectViewModel model)
        {
            var project = new Project();
            
            var mapResult = await model.MapToModel(project, _repository);
            if (!mapResult.Succeeded)
            {
                return BadRequest(mapResult.Errors);
            }

            var result = await _businesLogic.AddProjectAsync(project);
            if (result.Succeeded)
            {
                return Ok(new ProjectViewModel(project));
            }
            return BadRequest(result.Errors);
        }

        [HttpGet("{id}/option")]
        [ValidateProjectExists]
        public IActionResult GetProjectOptions(int id)
        {
            return Ok(
                _repository.ProjectOptions
                    .Where(po => po.ProjectId == id)
                    .Select(po => new ProjectOptionViewModel(po))
            );
        }

        [HttpPut("option")]
        [ValidateModel]
        public async Task<IActionResult> UpdateProjectOption([FromBody] ProjectOptionViewModel viewModel)
        {
            var option = _repository.ProjectOptions.SingleOrDefault(po => po.Id == viewModel.Id);
            if (option == null) return NotFound(viewModel.Id);
            var result = await viewModel.MapToModel(option, _repository);
            if (!result.Succeeded) return BadRequest(result.Errors);
            await _repository.SaveChangesAsync();
            return Ok(new ProjectOptionViewModel(option));
        }

        [HttpDelete("option/{id}")]
        public async Task<IActionResult> DeleteProjectOption(int id)
        {
            var option = _repository.ProjectOptions.SingleOrDefault(po => po.Id == id);
            if (option == null) return NotFound(id);
            var result = await _businesLogic.DeleteProjectOptionAsync(option);
            if (result.Succeeded) return Ok(id);
            return BadRequest(result.Errors);
        }

        
        public class ProjectUpdateRequest : IProjectUpdate
        {
            [Required]
            public int Id { get; set; }
            public string Name { get; set; }
            public string Summary { get; set; }
            public string Reference { get; set; }
            public string[] Categories { get; set; }
            public string OwningBusinessUnit { get; set; }
            public string ImpactedBusinessUnit { get; set; }
        }

        [HttpPut]
        [ValidateModel]
        public async Task<IActionResult> UpdateProject([FromBody] ProjectUpdateRequest[] requests)
        {
            var result = await _businesLogic.UpdateProjectAsync(requests);
            if (result.Succeeded)
                return Ok(result.GetData<IEnumerable<Project>>().Select(p => new ProjectViewModel(p)));
            return BadRequest(result.Errors);
        }

        [HttpPost("{id}/option")]
        [ValidateProjectExists]
        [ValidateModel]
        public async Task<IActionResult> CreateProjectOption(int id, [FromBody] ProjectOptionViewModel viewModel)
        {
            var project = _repository.Projects.Single(p => p.Id == id);
            var newOption = new ProjectOption
            {
                Project = project
            };

            var mapResult = await viewModel.MapToModel(newOption, _repository);
            if (!mapResult.Succeeded)
            {
                return BadRequest(mapResult.Errors);
            }

            // Check dependencies
            foreach (var dependency in viewModel.Dependencies)
            {
                var targetOption = _repository.ProjectOptions.SingleOrDefault(po => po.Id == dependency.OptionId);
                if (targetOption != null) continue;
                ModelState.AddModelError("Dependencies", $"The option with id {dependency.OptionId} does not exist.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _repository.AddProjectOptionAsync(newOption);
            if (viewModel.Dependencies == null) return Ok(new ProjectOptionViewModel(newOption));

            
            foreach (var dependency in viewModel.Dependencies)
            {
                var targetOption = _repository.ProjectOptions.Single(po => po.Id == dependency.OptionId);
                await _repository.AddProjectDependencyAsync(newOption, targetOption);
            }

            return Ok(new ProjectOptionViewModel(newOption));
        }


        [HttpPost("copy")]
        [ValidateModel]
        public async Task<IActionResult> CopyProject([FromBody] ProjectCopyRequest[] requests)
        {
            var result = await _businesLogic.CopyProjectAsync(requests);
            if (!result.Succeeded) return BadRequest(result.Errors);
            return Ok(result.GetData<IEnumerable<Project>>().Select(p => new ProjectViewModel(p)));
        }


    }

   
    
}
