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

        private readonly IMerlinPlanRepository _repository;
        private readonly IMerlinPlanBL _businessLogic;

        public ProjectOptionController(IMerlinPlanBL mpbl, IMerlinPlanRepository repo)
        {
            _repository = repo;
            _businessLogic = mpbl;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_repository.ProjectOptions.Select(po => new ProjectOptionViewModel(po)));

        [HttpGet("{id}/benefit")]
        [ValidateProjectOptionExists]
        public IActionResult GetOptionBenefits(int id)
        {
            var option = _repository.ProjectOptions.Single(po => po.Id == id);
            return Ok(option.Benefits.Select(b => new ProjectBenefitViewModel(b)));
        }

        [HttpPost("{id}/benefit")]
        [ValidateProjectOptionExists]
        [ValidateModel]
        public async Task<IActionResult> CreateOptionBenefit(int id, [FromBody] ProjectBenefitViewModel model)
        {
            var newBenefit = new ProjectBenefit();
            var mapResult = await model.MapToModel(newBenefit, _repository);
            if (!mapResult.Succeeded) return BadRequest(mapResult.Errors);
            var result = await _businessLogic.AddProjectBenefitAsync(newBenefit);
            if (result.Succeeded) return Ok(new ProjectBenefitViewModel(newBenefit));
            return BadRequest(result.Errors);
        }
    }
}