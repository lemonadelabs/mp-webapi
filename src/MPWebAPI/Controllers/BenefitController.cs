using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MPWebAPI.Filters;
using MPWebAPI.Models;
using MPWebAPI.ViewModels;

namespace MPWebAPI.Controllers
{
    [Route("api/[Controller]")]
    public class BenefitController : Controller
    {
        private readonly IMerlinPlanBL _businessLogic;
        private readonly IMerlinPlanRepository _repository;

        public BenefitController(IMerlinPlanBL mpbl, IMerlinPlanRepository repo)
        {
            _businessLogic = mpbl;
            _repository = repo;
        }

        [HttpGet]
        public IActionResult Get() => Ok(_repository.ProjectBenefits.Select(pb => new ProjectBenefitViewModel(pb)));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var benefit = _repository.ProjectBenefits.SingleOrDefault(b => b.Id == id);
            if(benefit == null) return NotFound(id);

            var result = await _businessLogic.DeleteProjectBenefitAsync(benefit);
            if (result.Succeeded) return Ok(id);
            return BadRequest(result.Errors);
        }

        [HttpPut]
        [ValidateModel]
        public async Task<IActionResult> Update([FromBody] ProjectBenefitViewModel model)
        {
            var option = _repository.ProjectOptions.SingleOrDefault(po => po.Id == model.ProjectOptionId);
            var benefit = option?.Benefits.SingleOrDefault(b => b.Id == model.Id);
            if (benefit == null) return NotFound(model.Id);
            var result = await model.MapToModel(benefit, _repository);
            if (!result.Succeeded) return BadRequest(result.Errors);
            await _repository.SaveChangesAsync();
            return Ok(new ProjectBenefitViewModel(benefit));
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] ProjectBenefitViewModel model)
        {
            var newBenefit = new ProjectBenefit();
            var mapResult = await model.MapToModel(newBenefit, _repository);
            if (!mapResult.Succeeded) return BadRequest(mapResult.Errors);
            var result = await _businessLogic.AddProjectBenefitAsync(newBenefit);
            if (result.Succeeded) return Ok(new ProjectBenefitViewModel(newBenefit));
            return BadRequest(result.Errors);
        }

        [HttpGet("{id}/alignment")]
        public IActionResult GetAlignment(int id)
        {
            var benefit = _repository.ProjectBenefits.SingleOrDefault(b => b.Id == id);
            if(benefit == null) return NotFound(id);
            return Ok(benefit.Alignments.Select(a => new AlignmentViewModel(a)));
        }
    }
}