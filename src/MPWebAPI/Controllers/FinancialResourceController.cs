using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MPWebAPI.Filters;
using MPWebAPI.Models;
using MPWebAPI.ViewModels;

namespace MPWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class FinancialResourceController : Controller
    {
        
        private readonly IMerlinPlanRepository _repository;
        private readonly IMerlinPlanBL _businessLogic;
        private readonly ILogger _logger;

        public FinancialResourceController(
            IMerlinPlanRepository repo, 
            IMerlinPlanBL mpbl, 
            ILoggerFactory loggerFactory)
        {
            _repository = repo;
            _businessLogic = mpbl;
            _logger = loggerFactory.CreateLogger<FinancialResourceCategory>();
        }

        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(
                _repository.FinancialResources
                .Select(fr => new FinancialResourceViewModel(fr))
                .ToList()
            );
        }
        
        [HttpDelete("{id}")]
        [ValidateFinancialResourceExists]
        public async Task<IActionResult> Delete(int id)
        {
            var financialResource = _repository.FinancialResources.First(fr => fr.Id == id);
            await _repository.RemoveFinancialResourceAsync(financialResource);
            return Ok();
        }

        [HttpPut]
        [ValidateModel]
        public async Task<IActionResult> Put([FromBody] FinancialResourceViewModel viewModel)
        {
            var financialResource = _repository.FinancialResources.FirstOrDefault(fr => fr.Id == viewModel.Id);
            
            if (financialResource == null)
            {
                return NotFound(viewModel.Id);
            }

            // Validate that resource scenario is valid
            var rs = _repository.ResourceScenarios.FirstOrDefault(s => s.Id == viewModel.ResourceScenarioId);
            if (rs == null)
            {
                return BadRequest(new { ResourceScenarioId = new [] {$"Resource Scenario not found with id {viewModel.ResourceScenarioId}"}});
            }

            viewModel.MapToModel(financialResource);
            await _repository.SaveChangesAsync();
            return Ok(new FinancialResourceViewModel(financialResource));
        }


        public class NewPartitionRequest : INewPartitionRequest
        {
            public string[] Categories { get; set; }
            public decimal StartingAjdustment { get; set; }
        }

        [HttpPost("{id}/partition")]
        [ValidateFinancialResourceExists]
        [ValidateModel]
        public async Task<IActionResult> CreatePartitions(int id, [FromBody] NewPartitionRequest[] request)
        {
            var resource = _repository.FinancialResources.First(rs => rs.Id == id);
            var result = await _businessLogic.AddFinancialResourcePartitionsAsync(resource, request);
            
            if (result.Succeeded)
            {
                return Ok();
            }
          
            return BadRequest(result.Errors);
        }
    }
}
