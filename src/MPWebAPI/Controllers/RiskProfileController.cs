using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MPWebAPI.Filters;
using MPWebAPI.Models;
using MPWebAPI.ViewModels;

namespace MPWebAPI.Controllers
{
    [Route("api/[Controller]")]
    public class RiskProfileController : Controller
    {
        private readonly IMerlinPlanRepository _repository;

        public RiskProfileController(IMerlinPlanRepository repo)
        {
            _repository = repo;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_repository.RiskProfiles.Select(rp => new RiskProfileViewModel(rp)));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var riskProfile = _repository.RiskProfiles.SingleOrDefault(rp => rp.Id == id);
            if (riskProfile == null) return NotFound(id);
            await _repository.RemoveRiskProfileAsync(riskProfile);
            return Ok(id);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] RiskProfileViewModel viewModel)
        {
            var newRiskProfile = new RiskProfile();
            var result = await viewModel.MapToModel(newRiskProfile, _repository);
            if (!result.Succeeded) return BadRequest(result.Errors);
            await _repository.AddRiskProfileAsync(newRiskProfile);
            return Ok(new RiskProfileViewModel(newRiskProfile));
        }

        [HttpPut]
        [ValidateModel]
        public async Task<IActionResult> Update([FromBody] RiskProfileViewModel viewModel)
        {
            var riskProfile = _repository.RiskProfiles.SingleOrDefault(rp => rp.Id == viewModel.Id);
            if (riskProfile == null) return NotFound(viewModel.Id);
            var result = await viewModel.MapToModel(riskProfile, _repository);
            if (!result.Succeeded) return BadRequest(result.Errors);
            await _repository.SaveChangesAsync();
            return Ok(new RiskProfileViewModel(riskProfile));
        }
    }
}