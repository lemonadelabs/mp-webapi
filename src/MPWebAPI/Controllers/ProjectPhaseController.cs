using System;
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
    public class ProjectPhaseController : ControllerBase
    {

        private readonly IMerlinPlanBL _businessLogic;
        private readonly IMerlinPlanRepository _repository;

        public ProjectPhaseController(IMerlinPlanRepository repo, IMerlinPlanBL mpbl)
        {
            _repository = repo;
            _businessLogic = mpbl;
        }

        [HttpGet]
        public IActionResult Get() 
            => Ok(_repository.ProjectPhases.Select(pp => new ProjectPhaseViewModel(pp)));

        [HttpGet("{id}")]
        [ValidateProjectPhaseExists]
        public IActionResult GetPhase(int id)
            => Ok(new ProjectPhaseViewModel(_repository.ProjectPhases.Single(pp => pp.Id == id)));

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> AddPhase([FromBody] ProjectPhaseViewModel viewModel)
        {
            var newPhase = new ProjectPhase();
            var mapResult = await viewModel.MapToModel(newPhase);
            if (!mapResult.Succeeded) return BadRequest(mapResult.Errors);
            var result = await _businessLogic.AddProjectPhaseAsync(newPhase);
            if (result.Succeeded) return Ok(new ProjectPhaseViewModel(newPhase));
            return BadRequest(result.Errors);
        }

        [HttpDelete("{id}")]
        [ValidateProjectPhaseExists]
        public async Task<IActionResult> Delete(int id)
        {
            var phase = _repository.ProjectPhases.Single(pp => pp.Id == id);
            await _businessLogic.DeleteProjectPhaseAsync(phase);
            return Ok(id);
        }

        [HttpPut]
        [ValidateModel]
        public async Task<IActionResult> Update([FromBody] ProjectPhaseViewModel viewModel)
        {
            var model = _repository.ProjectPhases.SingleOrDefault(pp => pp.Id == viewModel.Id);
            if (model == null) return NotFound(viewModel.Id);
            var result = await viewModel.MapToModel(model);
            if (!result.Succeeded) return BadRequest(result.Errors);
            await _repository.SaveChangesAsync();
            return Ok(new ProjectPhaseViewModel(model));
        }
    }
}
