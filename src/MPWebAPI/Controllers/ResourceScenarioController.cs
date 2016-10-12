using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MPWebAPI.Models;
using MPWebAPI.ViewModels;
using MPWebAPI.Filters;

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

        [HttpGet("useraccess/{id}")]
        [ValidateUserExists]
        public async Task<IActionResult> GetAllForUser(string id)
        {
            var user = _repository.Users.Where(u => u.Id == id).Single();
            var userShare = await _repository.GetUserSharedResourceScenariosForUserAsync(user);
            var groupShare = await _repository.GetGroupSharedResourceScenariosForUserAsync(user);
            var allShare = await _repository.GetOrganisationSharedResourceScenariosAsync(user.Organisation);
            
            var allPlusGroup = new List<ResourceScenario>();
            
            if (allShare != null)
            {
                allPlusGroup.AddRange(allShare);    
            }
          
            if (groupShare != null)
            {
                allPlusGroup.AddRange(groupShare);    
            }

            var groupSharedScenarios = new List<UserAccessResponse.GroupSharedScenario>();
            foreach (var rs in allPlusGroup)
            {
                var g = groupSharedScenarios.FirstOrDefault(gss => gss.Group.Id == rs.Group.Id);
                if (g != null)
                {
                    g.Scenarios.Add(new ResourceScenarioViewModel(rs));
                }
                else
                {
                    var newgss = new UserAccessResponse.GroupSharedScenario 
                    {
                        Group = new GroupViewModel(rs.Group),
                        Scenarios = new List<ResourceScenarioViewModel>(
                            new ResourceScenarioViewModel[] {new ResourceScenarioViewModel(rs)})
                    };
                }
            }

            var userSharedScenarios = new List<UserAccessResponse.UserSharedScenario>();
            foreach (var rs in userShare)
            {
                var u = userSharedScenarios.FirstOrDefault(uss => uss.User.UserName == rs.Creator.UserName);
                if (u != null)
                {
                    u.Scenarios.Add(new ResourceScenarioViewModel(rs));                    
                }
                else
                {
                    var newuss = new UserAccessResponse.UserSharedScenario
                    {
                        User = new UserViewModel(rs.Creator),
                        Scenarios = new List<ResourceScenarioViewModel>(
                            new ResourceScenarioViewModel[] { new ResourceScenarioViewModel(rs) })
                    };
                }
            }

            var owned = _repository.ResourceScenarios.Where(rs => rs.Creator.Id == id);

            return new JsonResult(
                new UserAccessResponse 
                {
                    Created = owned.Select(o => new ResourceScenarioViewModel(o)).ToList(),
                    GroupShare = groupSharedScenarios,
                    UserShare = userSharedScenarios
                }
            );
        }
    }
}
