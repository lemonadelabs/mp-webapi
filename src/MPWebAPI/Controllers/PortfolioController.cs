﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MPWebAPI.Filters;
using MPWebAPI.Models;
using MPWebAPI.ViewModels;

namespace MPWebAPI.Controllers
{
    [Route("api/[Controller]")]
    public class PortfolioController : ControllerBase
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

        [HttpGet("{id}/project")]
        [ValidatePortfolioExists]
        public IActionResult GetProjectsForPortfolio(int id)
        {
            return Ok(_repository.ProjectConfigs.Where(pc => pc.PortfolioId == id).Select(pc => new ProjectConfigViewModel(pc)));
        }

        [HttpDelete("{id}/project/{projectId}")]
        [ValidatePortfolioExists]
        public async Task<IActionResult> RemoveProjectFromPortfolio(int id, int projectId)
        {
            var projectConfig =
                _repository.ProjectConfigs.SingleOrDefault(pc => pc.PortfolioId == id && pc.Id == projectId);
            if (projectConfig == null) return NotFound(projectId);
            var result = await _businessLogic.RemoveProjectFromPortfolioAsync(projectConfig);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            return Ok(projectId);
        }


        [HttpGet]
        public IActionResult GetAll() => Ok(_repository.Portfolios.Select(p => new PortfolioViewModel(p)));

        [HttpGet("{id}")]
        [ValidatePortfolioExists]
        public IActionResult Get(int id) => Ok(new PortfolioViewModel(_repository.Portfolios.Single(p => p.Id == id)));


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
            var documents = new List<Portfolio>();

            var userShare = await _repository.GetUserSharedPortfoliosForUserAsync(user);
            var groupShare = await _repository.GetGroupSharedPortfoliosForUserAsync(user);
            var allShare = await _repository.GetOrganisationSharedPortfoliosAsync(user.Organisation);
            var owned = _repository.Portfolios.Where(rs => rs.Creator.Id == id);

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
                new AccessibleDocumentViewModel<PortfolioViewModel>
                {
                    Documents = documents.Select(d => new PortfolioViewModel(d)).ToList(),
                    Groups = groups.Select(g => new GroupViewModel(g)).ToList()
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
            foreach (var userId in userNameList.Users)
            {
                var u = _repository.Users.SingleOrDefault(user => user.Id == userId);
                if (u != null)
                {
                    users.Add(u);
                }
                else
                {
                    return BadRequest(new {Users = $"User {userId} does not exist."});
                }
            }

            foreach (var user in users)
            {
                await _repository.SharePortfolioWithUserAsync(ps, user);
            }
            return Ok(users.Select(async u =>
            {
                var vm = new UserViewModel();
                await vm.MapToViewModelAsync(u, _repository);
                return vm;
            }));
        }

        [HttpPut("{id}/user/unshare")]
        [ValidatePortfolioExists]
        public async Task<IActionResult> UnshareWithUser(int id, [FromBody] UserList userNameList)
        {
            var ps = _repository.Portfolios.Single(r => r.Id == id);
            var users = new List<MerlinPlanUser>();
            foreach (var userId in userNameList.Users)
            {
                var u = _repository.Users.SingleOrDefault(user => user.Id == userId);
                if (u != null)
                {
                    users.Add(u);
                }
                else
                {
                    return BadRequest(new {Users = $"User {userId} does not exist."});
                }
            }

            foreach (var user in users)
            {
                await _repository.UnsharePortfolioWithUserAsync(ps, user);
            }
            return Ok(users.Select(async u =>
            {
                var vm = new UserViewModel();
                await vm.MapToViewModelAsync(u, _repository);
                return vm;
            }));
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] PortfolioViewModel viewModel)
        {
            var newPortfolio = new Portfolio();
            var mapResult = await viewModel.MapToModel(newPortfolio, _repository);
            if (!mapResult.Succeeded) return BadRequest(mapResult.Errors);
            var result = await _businessLogic.AddPortfolioAsync(newPortfolio);
            if (result.Succeeded) return Ok(new PortfolioViewModel(newPortfolio));
            return BadRequest(result.Errors);
        }

        [HttpDelete("{id}")]
        [ValidatePortfolioExists]
        public async Task<IActionResult> Delete(int id)
        {
            var portfolio = _repository.Portfolios.Single(p => p.Id == id);
            var result = await _businessLogic.DeletePortfolioAsync(portfolio);
            if (result.Succeeded) return Ok(id);
            return BadRequest(result.Errors);
        }

        public class PortfolioUpdateRequest : IValidatableObject, IPortfolioUpdate
        {
            [Required]
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime? StartYear { get; set; }
            public DateTime? EndYear { get; set; }

            [Range(0, int.MaxValue)]
            public int? TimeScale { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                // Enddate must be later than start date
                if (EndYear < StartYear)
                {
                    yield return new ValidationResult(
                        "EndYear should not be before StartYear",
                        new [] {"EndYear"}
                    );
                }
            }
        }

        [HttpPut]
        [ValidateModel]
        public async Task<IActionResult> Update([FromBody] PortfolioUpdateRequest[] requests)
        {
            var result = await _businessLogic.UpdatePortfolioAsync(requests);
            if (result.Succeeded) return Ok(result.GetData<IEnumerable<Portfolio>>().Select(p => new PortfolioViewModel(p)));
            return BadRequest(result.Errors);
        }

        public class AddProjectRequest : IAddProjectToPortfolioRequest
        {
            [Required]
            public int ProjectId { get; set; }

            [Required]
            public int OptionId { get; set; }

            public string[] Tags { get; set; }
            public DateTime? EstimatedStartDate { get; set; }
            public int? Owner { get; set; }
            public int[] Managers { get; set; }
        }

        [HttpPost("{id}/project")]
        [ValidatePortfolioExists]
        public async Task<IActionResult> AddProjects(int id, [FromBody] AddProjectRequest[] requests)
        {
            var result =
                await
                    _businessLogic.AddProjectToPortfolioAsync(_repository.Portfolios.Single(p => p.Id == id), requests);
            if (result.Succeeded)
                return Ok(result.GetData<IEnumerable<ProjectConfig>>().Select(pc => new ProjectConfigViewModel(pc)));
            return BadRequest(result.Errors);
        }

        [HttpPut("project")]
        [ValidateModel]
        public async Task<IActionResult> UpdateProject([FromBody] ProjectConfigViewModel viewModel)
        {
            var projectConfig = _repository.ProjectConfigs.SingleOrDefault(pc => pc.Id == viewModel.Id);
            if (projectConfig == null) return NotFound(viewModel.Id);

            var mapResult = await viewModel.MapToModel(projectConfig, _repository);
            if (!mapResult.Succeeded) return BadRequest(mapResult.Errors);
            var result = await _businessLogic.UpdatePortfolioProjectAsync(projectConfig);
            if (!result.Succeeded) return BadRequest(result.Errors);
            return Ok(new ProjectConfigViewModel(projectConfig));
        }

        [HttpGet("{id}/validate")]
        [ValidatePortfolioExists]
        public async Task<IActionResult> ValidatePortfolio(int id)
        {
            var portfolio = _repository.Portfolios.Single(p => p.Id == id);
            var result = await _businessLogic.ValidatePortfolioAsync(portfolio);
            if (result.Succeeded) return Ok();
            return BadRequest(result.Errors);
        }
    }
}