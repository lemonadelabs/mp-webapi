using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MPWebAPI.Models;
using MPWebAPI.ViewModels;

namespace MPWebAPI.Controllers
{
    [Route("api/[Controller]")]
    public class AlignmentController : Controller
    {
        private readonly IMerlinPlanBL _businessLogic;
        private readonly IMerlinPlanRepository _repository;

        public AlignmentController(IMerlinPlanBL mpbl, IMerlinPlanRepository repo)
        {
            _businessLogic = mpbl;
            _repository = repo;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_repository.Alignments.Select(a => new AlignmentViewModel(a)));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var alignment = _repository.Alignments.SingleOrDefault(a => a.Id == id);
            if (alignment == null) return NotFound(id);
            await _repository.RemoveAlignment(alignment);
            return Ok(id);
        }






    }
}