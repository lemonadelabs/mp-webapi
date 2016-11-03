using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MPWebAPI.Filters;
using MPWebAPI.Models;
using MPWebAPI.ViewModels;

namespace MPWebAPI.Controllers
{
    [Route("api/[Controller]")]
    public class StaffResourceController : Controller
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

        [HttpDelete("{id")]
        [ValidateStaffResourceExists]
        public async Task<IActionResult> DeleteStaffResource(int id)
        {
            var resource = _repository.StaffResources.Single(sr => sr.Id == id);
            await _repository.RemoveStaffResourceAsync(resource);
            return Ok(id);
        }



    }
}
