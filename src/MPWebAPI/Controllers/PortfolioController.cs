using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MPWebAPI.Filters;
using MPWebAPI.Models;
using MPWebAPI.ViewModels;

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

        [HttpGet]
        public IActionResult GetAll() => Ok(_repository.Portfolios.Select(p => new PortfolioViewModel(p)));

        [HttpGet("group/{id}")]
        [ValidateGroupExists]
        public IActionResult GetAllForGroup(int id)
        {
            return Ok(_repository.Portfolios.Where(p => p.Group.Id == id).Select(pr => new PortfolioViewModel(pr)));
        }

        [HttpGet("user/{id}")]
        [ValidateUserExists]
        public IActionResult GetAllForUser(string id)
        {
            return Ok(_repository.Portfolios.Where(p => p.Creator.Id == id).Select(pf => new PortfolioViewModel(pf)));
        }




    }
}