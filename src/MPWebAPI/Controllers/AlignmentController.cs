using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MPWebAPI.Filters;
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
            await _repository.RemoveAlignmentAsync(alignment);
            return Ok(id);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AlignmentViewModel viewModel)
        {
            var newAlignment = new Alignment();
            var mapResult = await viewModel.MapToModel(newAlignment, _repository);
            if (!mapResult.Succeeded) return BadRequest(mapResult.Errors);
            await _repository.AddAlignmentAsync(newAlignment);
            return Ok(new AlignmentViewModel(newAlignment));
        }

        [HttpPut]
        [ValidateModel]
        public async Task<IActionResult> Update([FromBody] AlignmentViewModel viewModel)
        {
            var alignment = _repository.Alignments.SingleOrDefault(a => a.Id == viewModel.Id);
            if (alignment == null) return NotFound(viewModel.Id);
            var mapResult = await viewModel.MapToModel(alignment, _repository);
            if (!mapResult.Succeeded) return BadRequest(mapResult.Errors);
            await _repository.SaveChangesAsync();
            return Ok(new AlignmentViewModel(alignment));
        }
    }
}