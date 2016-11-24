using System.Linq;
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


    }
}