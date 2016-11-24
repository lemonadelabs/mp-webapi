using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
    }
}