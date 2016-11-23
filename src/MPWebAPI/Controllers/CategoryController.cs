using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MPWebAPI.Models;
using MPWebAPI.ViewModels;
using MPWebAPI.Filters;

namespace MPWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly IMerlinPlanRepository _repository;
        private readonly IMerlinPlanBL _businessLogic;
        
        public CategoryController(IMerlinPlanRepository repo, IMerlinPlanBL mpbl)
        {
            _repository = repo;
            _businessLogic = mpbl;
        }

        [HttpGet("businessunit/{id}")]
        [ValidateBusinessUnitExists]
        public IActionResult GetBusinessUnit(int id)
        {
            return Ok(new BusinessUnitViewModel(_repository.BusinessUnits.Single(bu => bu.Id == id)));
        }

        [HttpDelete("businessunit/{id}")]
        [ValidateBusinessUnitExists]
        public async Task<IActionResult> DeleteBusinessUnit(int id)
        {
            var bu = _repository.BusinessUnits.Single(b => b.Id == id);
            var result = await _businessLogic.DeleteBusinessUnitAsync(bu);
            if (result.Succeeded)
            {
                return Ok(id);
            }
            return BadRequest(result.Errors);
        }

        [HttpGet("financialresource/{id}")]
        [ValidateFinancialResourceCategoryExists]
        public IActionResult GetFinancialResourceCategory(int id)
        {
            return new JsonResult(
                new FinancialResourceCategoryViewModel(
                    _repository.FinancialResourceCategories.First(frc => frc.Id == id)
                    )
                );
        }

        [HttpDelete("financialresource/{id}")]
        [ValidateFinancialResourceCategoryExists]
        public async Task<IActionResult> DeleteFinancialResourceCategory(int id)
        {
            var frc = _repository.FinancialResourceCategories.First(rc => rc.Id == id);

            var result = await _businessLogic.DeleteFinancialResourceCategoryAsync(frc);
            if (result.Succeeded)
            {
                return Ok(id);
            }
            return BadRequest(result.Errors);
        }

        [HttpGet("benefit/{id}")]
        public IActionResult GetBenefit(int id)
        {
            var cat = _repository.BenefitCategories.SingleOrDefault(bc => bc.Id == id);
            if (cat == null)
            {
                return NotFound(id);
            }
            return Ok(new BenefitCategoryViewModel(cat));
        }

        [HttpDelete("benefit/{id}")]
        public async Task<IActionResult> DeleteBenefitCategory(int id)
        {
            var cat = _repository.BenefitCategories.SingleOrDefault(bc => bc.Id == id);
            if (cat == null)
            {
                return NotFound(id);
            }
            var result = await _businessLogic.DeleteBenefitCategoryAsync(cat);
            if (result.Succeeded) return Ok(id);
            return BadRequest(result.Errors);
        }
    }
}
