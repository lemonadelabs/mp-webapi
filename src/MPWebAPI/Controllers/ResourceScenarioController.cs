using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MPWebAPI.Models;
using MPWebAPI.ViewModels;

namespace MPWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class ResourceScenarioController : Controller
    {
        
        private readonly IMerlinPlanRepository _repository;
        private readonly IMerlinPlanBL _businessLogic;
        private readonly ILogger _logger;
        
        public ResourceScenarioController(
            IMerlinPlanRepository repository, 
            IMerlinPlanBL businessLogic,
            ILoggerFactory loggerFactory
            )
        {
            _repository = repository;
            _businessLogic = businessLogic;
            _logger = loggerFactory.CreateLogger<ResourceScenarioController>();
        }
        
        /// <summary>
        /// Get all resource scenarios in the system. 
        /// </summary>
        [HttpGet]
        public IEnumerable<ResourceScenarioViewModel> Get()
        {
            return _repository.ResourceScenarios.Select(rs => new ResourceScenarioViewModel(rs));
        }
    }
}
