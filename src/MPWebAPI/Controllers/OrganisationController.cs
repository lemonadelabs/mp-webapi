using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MPWebAPI.Models;
using MPWebAPI.ViewModels;

namespace MPWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class OrganisationController : Controller
    {
        private readonly IMerlinPlanRepository _mprepo;
        private readonly IMerlinPlanBL _mpbl;

        public OrganisationController(IMerlinPlanRepository mprepo, IMerlinPlanBL mpbl)
        {
            _mprepo = mprepo;
            _mpbl = mpbl;
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
