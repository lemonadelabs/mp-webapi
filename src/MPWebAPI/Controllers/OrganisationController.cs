using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MPWebAPI.Filters;
using MPWebAPI.Models;
using MPWebAPI.ViewModels;

namespace MPWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class OrganisationController : ControllerBase
    {
        private readonly IMerlinPlanRepository _mprepo;
        private readonly IMerlinPlanBL _mpbl;

        public OrganisationController(IMerlinPlanRepository mprepo, IMerlinPlanBL mpbl)
        {
            _mprepo = mprepo;
            _mpbl = mpbl;
        }
        
        
        [HttpGet("{id}/group")]
        [ValidateOrganisationExists]
        public IActionResult GetGroups(int id)
        {
            return new JsonResult(
                _mprepo.GetOrganisationGroups(
                    _mprepo.Organisations.Single(o => o.Id == id)).Select(g => new GroupViewModel(g)));
        }

        [HttpGet("{id}/user")]
        [ValidateOrganisationExists]
        public IActionResult GetUsers(int id)
        {
            var users = _mprepo.Users.ToList()
                .Where(u => u.OrganisationId == id);
            var viewModels = users
                .Select(async u => 
                    { 
                        var vm = new UserViewModel();
                        await vm.MapToViewModelAsync(u, _mprepo);
                        return vm;
                    })
                .Select(uvm => uvm.Result);
            return new JsonResult(viewModels);
        }

        [HttpGet]
        public IEnumerable<OrganisationViewModel> GetAll()
        {
             return _mprepo.Organisations.Select(o => new OrganisationViewModel(o));
        }

        [HttpGet("{id}")]
        [ValidateOrganisationExists]
        public IActionResult Get(int id)
        {
            return new JsonResult(new OrganisationViewModel(_mprepo.Organisations.Single(o => o.Id == id)));
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Post([FromBody] OrganisationViewModel orgvm)
        {
            var newOrg = new Organisation();
            await orgvm.MapToModel(newOrg);
            await _mpbl.CreateOrganisation(newOrg);
            return Ok();    
        }

        [HttpDelete("{id}")]
        [ValidateOrganisationExists]
        public async Task<IActionResult> Delete(int id)
        {
            await _mprepo.RemoveOrganisationAsync(_mprepo.Organisations.Single(o => o.Id == id));
            return Ok();
        }

        [HttpPut]
        [ValidateModel]
        public async Task<IActionResult> Update([FromBody] OrganisationViewModel orgvm)
        {
            var org = _mprepo.Organisations.FirstOrDefault(o => o.Id == orgvm.Id);
            if (org == null) return NotFound();
            await orgvm.MapToModel(org);
            await _mprepo.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{id}/businessunit")]
        [ValidateOrganisationExists]
        public IActionResult GetBusinessUnits(int id)
        {
            return Ok(
                _mprepo.BusinessUnits
                    .Where(bu => bu.OrganisationId == id)
                    .Select(bu => new BusinessUnitViewModel(bu))
                    );
        }

        [HttpPost("{id}/businessunit")]
        [ValidateOrganisationExists]
        [ValidateModel]
        public async Task<IActionResult> AddBusinessUnit(int id, [FromBody] BusinessUnitViewModel model)
        {
            var businessUnit = new BusinessUnit();
            await model.MapToModel(businessUnit);
            businessUnit.OrganisationId = id;
            var result = await _mpbl.AddBusinessUnitAsync(businessUnit);
            if (!result.Succeeded) return BadRequest(result.Errors);
            return Ok(new BusinessUnitViewModel(businessUnit));
        }
    }
}
