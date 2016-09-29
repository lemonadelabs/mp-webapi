using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MPWebAPI.Filters;
using MPWebAPI.Models;
using MPWebAPI.ViewModels;

namespace MPWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class OrganisationController : MerlinPlanController
    {
        private readonly IMerlinPlanRepository _mprepo;
        private readonly IMerlinPlanBL _mpbl;
        private readonly UserManager<MerlinPlanUser> _userManager;

        public OrganisationController(IMerlinPlanRepository mprepo, IMerlinPlanBL mpbl, UserManager<MerlinPlanUser> userManager)
        {
            _mprepo = mprepo;
            _mpbl = mpbl;
            _userManager = userManager;
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
            var users = _userManager.Users.ToList()
                .Where(u => u.OrganisationId == id);
            
            var viewModels = new List<UserViewModel>();
            
            foreach (var u in users)
            {
                var uvm = new UserViewModel(u);
                uvm.Roles = _userManager.GetRolesAsync(u).Result;
                viewModels.Add(uvm);
            }
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
            orgvm.MapToModel(newOrg);
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
            if (org != null)
            {
                orgvm.MapToModel(org);
                await _mprepo.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
