using Microsoft.AspNetCore.Mvc;
using MPWebAPI.Models;

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
    }
}
