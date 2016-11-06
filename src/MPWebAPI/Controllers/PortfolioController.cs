using Microsoft.AspNetCore.Mvc;
using MPWebAPI.Models;

namespace MPWebAPI.Controllers
{
    [Route("api/[Controller]")]
    public class PortfolioController : Controller
    {
        private readonly IMerlinPlanRepository _repository;
        private readonly IMerlinPlanBL _businessLogic;

        public PortfolioController(IMerlinPlanBL mpbl, IMerlinPlanRepository repo)
        {
            _repository = repo;
            _businessLogic = mpbl;
        }
    }
}