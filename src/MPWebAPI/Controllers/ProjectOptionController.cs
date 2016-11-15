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
    public class ProjectOptionController : Controller
    {

        private readonly IMerlinPlanBL _businessLogic;
        private readonly IMerlinPlanRepository _repository;

        public ProjectOptionController(IMerlinPlanRepository repo, IMerlinPlanBL mpbl)
        {
            _repository = repo;
            _businessLogic = mpbl;
        }

        [HttpGet]
        public IActionResult Get() 
            => Ok(_repository.ProjectOptions.Select(po => new ProjectOptionViewModel(po)));

        [HttpGet("{id}/phase")]
        [ValidateProjectOptionExists]
        public IActionResult GetPhases(int id)
            => Ok(_repository.ProjectOptions.Single(po => po.Id == id).Phases?.Select(p => new ProjectPhaseViewModel(p)));

        //[HttpPost("{id}/phase")]
        //[ValidateProjectOptionExists]
        //public async Task<IActionResult> AddPhase(int id, [FromBody] ProjectPhaseViewModel viewModel)
        //{
        //    var newPhase = new ProjectPhase();
        //    var mapResult = await viewModel.MapToModel(newPhase);
        //    if (!mapResult.Succeeded) return BadRequest(mapResult.Errors);


        //}

    }
}
