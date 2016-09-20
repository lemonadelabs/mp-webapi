using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MPWebAPI.Models;
using MPWebAPI.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MPWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class OrganisationController : Controller
    {
        
        private IMerlinPlanRepository _mprepo;

        public OrganisationController(IMerlinPlanRepository mprepo)
        {
            _mprepo = mprepo;
        }
        
        [HttpGet]
        public IEnumerable<OrganisationViewModel> Get()
        {
             return _mprepo.Organisations.Select(o => new OrganisationViewModel(o));
        }

        [HttpPost]
        public async Task Post([FromBody]OrganisationViewModel orgvm)
        {
            // Some validation here!
            var newOrg = new Organisation();
            orgvm.MapToModel(newOrg);
            await _mprepo.AddOrganisation(newOrg);
        }
    }
}
