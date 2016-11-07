using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MPWebAPI.Models;
using MPWebAPI.ViewModels;

namespace MPWebAPI.Controllers
{
    [Route("api/[Controller]")]
    public class ProjectController : Controller
    {
        private readonly IMerlinPlanBL _businesLogic;
        private readonly IMerlinPlanRepository _repository;

        public ProjectController(IMerlinPlanRepository repo, IMerlinPlanBL mpbl)
        {
            _businesLogic = mpbl;
            _repository = repo;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_repository.Projects.Select(p => new ProjectViewModel(p)));
    }
}
