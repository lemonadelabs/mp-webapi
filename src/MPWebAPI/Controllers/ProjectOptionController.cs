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

        [HttpGet("{id}/riskprofile")]
        [ValidateProjectOptionExists]
        public IActionResult GetRiskProfile(int id)
        {
            return Ok(
                _repository.RiskProfiles
                    .Where(rp => rp.ProjectOptionId == id)
                    .Select(rp => new RiskProfileViewModel(rp))
            );
        }
    }
}