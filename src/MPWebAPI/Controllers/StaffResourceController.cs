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
    public class StaffResourceController : ControllerBase
    {
        private readonly IMerlinPlanBL _businessLogic;
        private readonly IMerlinPlanRepository _repository;

        public StaffResourceController(IMerlinPlanRepository repo, IMerlinPlanBL mpbl)
        {
            _businessLogic = mpbl;
            _repository = repo;
        }

        [HttpGet]
        public IActionResult GetAll() => new JsonResult(
            _repository.StaffResources
                .Select(sr => new StaffResourceViewModel(sr))
                .ToList()
            );

        [HttpGet("{id}")]
        [ValidateStaffResourceExists]
        public IActionResult Get(int id) => new JsonResult(
                new StaffResourceViewModel(_repository.StaffResources.Single(sr => sr.Id == id))
            );

        [HttpGet("{id}/adjustment")]
        [ValidateStaffResourceExists]
        public IActionResult GetAdjustments(int id)
        {
            var resource = _repository.StaffResources.Single(sr => sr.Id == id);
            return new JsonResult(resource.Adjustments.Select(a => new StaffAdjustmentViewModel(a)).ToList());
        }

        [HttpDelete("{id}")]
        [ValidateStaffResourceExists]
        public async Task<IActionResult> DeleteStaffResource(int id)
        {
            var resource = _repository.StaffResources.Single(sr => sr.Id == id);
            await _repository.RemoveStaffResourceAsync(resource);
            return Ok(id);
        }

        [HttpPut]
        [ValidateModel]
        public async Task<IActionResult> UpdateStaffResource([FromBody] StaffResourceViewModel model)
        {
            var resource = _repository.StaffResources.SingleOrDefault(sr => sr.Id == model.Id);
            if (resource == null) return NotFound(model.Id);

            var scenario = _repository.ResourceScenarios.FirstOrDefault(rs => rs.Id == model.ResourceScenarioId);
            if (scenario == null) return NotFound($"The resource scenario {model.ResourceScenarioId} can't be found");

            await model.MapToModel(resource, _repository);

            var result = await _businessLogic.UpdateStaffResourceAsync(resource);
            if (result.Succeeded)
            {
                return Ok(new StaffResourceViewModel(resource));
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("copy")]
        [ValidateModel]
        public async Task<IActionResult> CopyStaffResources([FromBody] CopyRequest[] requests)
        {
            var result = await _businessLogic.CopyStaffResourcesAsync(requests);
            if (result.Succeeded)
            {
                return Ok(result.GetData<IEnumerable<StaffResource>>().Select(sr => new StaffResourceViewModel(sr)));
            }
            return BadRequest(result.Errors);
        }
    }
}
