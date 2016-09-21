using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MPWebAPI.Models;
using MPWebAPI.ViewModels;

namespace MPWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class OrganisationController : Controller
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
        public IActionResult GetGroups(int id)
        {
            var org = _mprepo.Organisations.FirstOrDefault(o => o.Id == id);
            if (org != null)
            {
                return new JsonResult(_mprepo.GetOrganisationGroups(org).Select(g => new GroupViewModel(g)));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{id}/user")]
        public IActionResult GetUsers(int id)
        {
            var org = _mprepo.Organisations.FirstOrDefault(o => o.Id == id);
            if (org != null)
            {
                var users = _userManager.Users.ToList()
                    .Where(u => u.OrganisationId == org.Id);
                
                var viewModels = new List<UserViewModel>();
                
                foreach (var u in users)
                {
                    var uvm = new UserViewModel(u);
                    uvm.Roles = _userManager.GetRolesAsync(u).Result;
                    viewModels.Add(uvm);
                }
                return new JsonResult(viewModels);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IEnumerable<OrganisationViewModel> GetAll()
        {
             return _mprepo.Organisations.Select(o => new OrganisationViewModel(o));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
             var org = _mprepo.Organisations.FirstOrDefault(o => o.Id == id);
             if (org != null)
             {
                 return new JsonResult(new OrganisationViewModel(org));
             }
             else
             {
                 return NotFound();
             }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrganisationViewModel orgvm)
        {
            if (ModelState.IsValid)
            {
                var newOrg = new Organisation();
                orgvm.MapToModel(newOrg);
                await _mpbl.CreateOrganisation(newOrg);
                return Ok();    
            }
            else 
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var org = _mprepo.Organisations.FirstOrDefault(o => o.Id == id);
            if (org != null)
            {
                await _mprepo.RemoveOrganisation(org);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> Update([FromBody] OrganisationViewModel orgvm)
        {
            if (ModelState.IsValid)
            {
                var org = _mprepo.Organisations.FirstOrDefault(o => o.Id == orgvm.Id);
                if (org != null)
                {
                    orgvm.MapToModel(org);
                    await _mprepo.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
