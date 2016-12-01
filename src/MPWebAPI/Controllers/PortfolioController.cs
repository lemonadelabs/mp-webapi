using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MPWebAPI.Filters;
using MPWebAPI.Models;
using MPWebAPI.ViewModels;

namespace MPWebAPI.Controllers
{
    [Route("api/[Controller]")]
    public class PortfolioController : Controller
    {
        private readonly IMerlinPlanRepository _repository;
        private readonly IMerlinPlanBL _businessLogic;

        public class UserList
        {
            public IEnumerable<string> Users { get; set; }
        }

        public PortfolioController(IMerlinPlanBL mpbl, IMerlinPlanRepository repo)
        {
            _repository = repo;
            _businessLogic = mpbl;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_repository.Portfolios.Select(p => new PortfolioViewModel(p)));

        [HttpGet("group/{id}")]
        [ValidateGroupExists]
        public IActionResult GetAllForGroup(int id)
        {
            return Ok(_repository.Portfolios.Where(p => p.Group.Id == id).Select(pr => new PortfolioViewModel(pr)));
        }

        [HttpGet("user/{id}")]
        [ValidateUserExists]
        public IActionResult GetAllByUser(string id)
        {
            return Ok(_repository.Portfolios.Where(p => p.Creator.Id == id).Select(pf => new PortfolioViewModel(pf)));
        }

        [HttpGet("useraccess/{id}")]
        [ValidateUserExists]
        public async Task<IActionResult> GetAllForUser(string id)
        {
            var user = _repository.Users.Single(u => u.Id == id);
            var userShare = await _repository.GetUserSharedPortfoliosForUserAsync(user);
            var groupShare = await _repository.GetGroupSharedPortfoliosForUserAsync(user);
            var allShare = await _repository.GetOrganisationSharedPortfoliosAsync(user.Organisation);
            var owned = _repository.Portfolios.Where(rs => rs.Creator.Id == id);

            var groupSharedScenarios = AccessibleDocumentViewModel<PortfolioViewModel>.DocumentsByGroup(groupShare.ToList());
            var orgSharedScenarios = AccessibleDocumentViewModel<PortfolioViewModel>.DocumentsByGroup(allShare.ToList());

            var userSharedScenarios = new List<AccessibleDocumentViewModel<PortfolioViewModel>.UserShared>();
            foreach (var rs in userShare)
            {
                var u = userSharedScenarios.FirstOrDefault(uss => uss.User.UserName == rs.Creator.UserName);
                if (u != null)
                {
                    u.Documents.Add(new PortfolioViewModel(rs));
                }
                else
                {
                    var uvm = new UserViewModel();
                    await uvm.MapToViewModelAsync(rs.Creator, _repository);
                    var newuss = new AccessibleDocumentViewModel<PortfolioViewModel>.UserShared
                    {
                        User = uvm,
                        Documents = new List<PortfolioViewModel>(
                            new[] { new PortfolioViewModel(rs) })
                    };
                    userSharedScenarios.Add(newuss);
                }
            }

            return Ok(
                new AccessibleDocumentViewModel<PortfolioViewModel>
                {
                    Created = owned.Select(o => new PortfolioViewModel(o)).ToList(),
                    GroupShare = groupSharedScenarios,
                    OrgShare = orgSharedScenarios,
                    UserShare = userSharedScenarios
                }
            );
        }


        [HttpPut("{id}/group/share")]
        [ValidatePortfolioExists]
        public async Task<IActionResult> GroupShare(int id)
        {
            var portfolio = _repository.Portfolios.Single(p => p.Id == id);
            await _repository.SharePortfolioWithGroupAsync(portfolio, true);
            return Ok(new PortfolioViewModel(portfolio));
        }

        [HttpPut("{id}/group/unshare")]
        [ValidatePortfolioExists]
        public async Task<IActionResult> GroupUnshare(int id)
        {
            var portfolio = _repository.Portfolios.Single(p => p.Id == id);
            await _repository.SharePortfolioWithGroupAsync(portfolio, false);
            return Ok(new PortfolioViewModel(portfolio));
        }

        [HttpPut("{id}/share")]
        [ValidatePortfolioExists]
        public async Task<IActionResult> Share(int id)
        {
            var portfolio = _repository.Portfolios.Single(p => p.Id == id);
            await _repository.SharePortfolioWithOrgAsync(portfolio, true);
            return Ok(new PortfolioViewModel(portfolio));
        }

        [HttpPut("{id}/unshare")]
        [ValidatePortfolioExists]
        public async Task<IActionResult> Unshare(int id)
        {
            var portfolio = _repository.Portfolios.Single(p => p.Id == id);
            await _repository.SharePortfolioWithOrgAsync(portfolio, false);
            return Ok(new PortfolioViewModel(portfolio));
        }


        [HttpPut("{id}/user/share")]
        [ValidatePortfolioExists]
        public async Task<IActionResult> ShareWithUser(int id, [FromBody] UserList userNameList)
        {
            var ps = _repository.Portfolios.Single(r => r.Id == id);
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
                await _repository.SharePortfolioWithUserAsync(ps, user);
            }
            return Ok();
        }

        [HttpPut("{id}/user/unshare")]
        [ValidatePortfolioExists]
        public async Task<IActionResult> UnshareWithUser(int id, [FromBody] UserList userNameList)
        {
            var ps = _repository.Portfolios.Single(r => r.Id == id);
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
                await _repository.UnsharePortfolioWithUserAsync(ps, user);
            }
            return Ok();
        }
    }
}