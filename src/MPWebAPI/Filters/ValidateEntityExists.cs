using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MPWebAPI.Models;

namespace MPWebAPI.Filters
{
    public class ValidateOrganisationExistsAttribute : TypeFilterAttribute
    {
        public ValidateOrganisationExistsAttribute() : base(typeof(ValidateOrganisationExists)) {}
        
        private class ValidateOrganisationExists : IAsyncActionFilter
        {
            private readonly IMerlinPlanRepository _repository;
            
            public ValidateOrganisationExists(IMerlinPlanRepository mprepo)
            {
                _repository = mprepo;
            }
            
            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (context.ActionArguments.ContainsKey("id"))
                {
                    var id = context.ActionArguments["id"] as int?;
                    if (id.HasValue)
                    {
                        if (await _repository.Organisations.ToAsyncEnumerable().All(o => o.Id != id.Value))
                        {
                            context.Result = new NotFoundObjectResult(id.Value);
                            return;
                        }
                        else
                        {
                             await next();
                        }
                    }
                }
                else
                {
                    context.Result = new NotFoundResult();
                    return;
                }
            }
        }
    }


    public class ValidateGroupExistsAttribute : TypeFilterAttribute
    {
        public ValidateGroupExistsAttribute() : base(typeof(ValidateGroupExists)) {}
        
        private class ValidateGroupExists : IAsyncActionFilter
        {
            private readonly IMerlinPlanRepository _repository;
            
            public ValidateGroupExists(IMerlinPlanRepository mprepo)
            {
                _repository = mprepo;
            }
            
            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (context.ActionArguments.ContainsKey("id"))
                {
                    var id = context.ActionArguments["id"] as int?;
                    if (id.HasValue)
                    {
                        if (await _repository.Groups.ToAsyncEnumerable().All(o => o.Id != id.Value))
                        {
                            context.Result = new NotFoundObjectResult(id.Value);
                            return;
                        }
                        else
                        {
                            await next();            
                        }
                    }
                }
                else
                {
                    context.Result = new NotFoundResult();
                    return;
                }
            }
        }
    }


    public class ValidateUserExistsAttribute : TypeFilterAttribute
    {
        public ValidateUserExistsAttribute() : base(typeof(ValidateUserExists)) {}
        
        private class ValidateUserExists : IAsyncActionFilter
        {
            private readonly UserManager<MerlinPlanUser> _userManager;
            
            public ValidateUserExists(UserManager<MerlinPlanUser> userManager)
            {
                _userManager = userManager;
            }
            
            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (context.ActionArguments.ContainsKey("id"))
                {
                    var id = context.ActionArguments["id"] as string;
                    if (id != null)
                    {
                        if (await _userManager.Users.ToAsyncEnumerable().All(u => u.Id != id))
                        {
                            context.Result = new NotFoundObjectResult(id);
                            return;
                        }
                        else
                        {
                            await next();            
                        }
                    }
                }
                else
                {
                    context.Result = new NotFoundResult();
                    return;                    
                }
            }
        }
    }    
}