using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MPWebAPI.Filters;
using MPWebAPI.Models;

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
        
        [HttpDelete("{id}")]
        [ValidateFinancialResourceExists]
        public async Task<IActionResult> Delete(int id)
        {
            var financialResource = _repository.FinancialResources.First(fr => fr.Id == id);
            await _repository.RemoveFinancialResourceAsync(financialResource);
            return Ok();
        }

    }
}
