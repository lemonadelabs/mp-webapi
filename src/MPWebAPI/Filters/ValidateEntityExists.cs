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
        public ValidateOrganisationExistsAttribute() : base(typeof(ValidateOrganisationExists))
        {
        }

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
                        await next();
                    }
                }
                else
                {
                    context.Result = new NotFoundResult();
                }
            }
        }
    }

    public class ValidateProjectExistsAttribute : TypeFilterAttribute
    {
        public ValidateProjectExistsAttribute() : base(typeof(ValidateProjectExists))
        {
        }

        private class ValidateProjectExists : IAsyncActionFilter
        {
            private readonly IMerlinPlanRepository _repository;

            public ValidateProjectExists(IMerlinPlanRepository mprepo)
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
                        if (await _repository.Projects.ToAsyncEnumerable().All(o => o.Id != id.Value))
                        {
                            context.Result = new NotFoundObjectResult(id.Value);
                            return;
                        }
                        await next();
                    }
                }
                else
                {
                    context.Result = new NotFoundResult();
                }
            }
        }
    }


    public class ValidateGroupExistsAttribute : TypeFilterAttribute
    {
        public ValidateGroupExistsAttribute() : base(typeof(ValidateGroupExists))
        {
        }

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
                        await next();
                    }
                }
                else
                {
                    context.Result = new NotFoundResult();
                }
            }
        }
    }

    public class ValidateResourceScenarioExistsAttribute : TypeFilterAttribute
    {
        public ValidateResourceScenarioExistsAttribute() : base(typeof(ValidateResourceScenarioExists))
        {
        }

        private class ValidateResourceScenarioExists : IAsyncActionFilter
        {
            private readonly IMerlinPlanRepository _repository;

            public ValidateResourceScenarioExists(IMerlinPlanRepository mprepo)
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
                        if (await _repository.ResourceScenarios.ToAsyncEnumerable().All(o => o.Id != id.Value))
                        {
                            context.Result = new NotFoundObjectResult(id.Value);
                            return;
                        }

                        await next();
                    }
                }
                else
                {
                    context.Result = new NotFoundResult();
                }
            }
        }
    }

    public class ValidateFinancialResourceExistsAttribute : TypeFilterAttribute
    {
        public ValidateFinancialResourceExistsAttribute() : base(typeof(ValidateFinancialResourceExists))
        {
        }

        private class ValidateFinancialResourceExists : IAsyncActionFilter
        {
            private readonly IMerlinPlanRepository _repository;

            public ValidateFinancialResourceExists(IMerlinPlanRepository mprepo)
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
                        if (await _repository.FinancialResources.ToAsyncEnumerable().All(o => o.Id != id.Value))
                        {
                            context.Result = new NotFoundObjectResult(id.Value);
                            return;
                        }

                        await next();
                    }
                }
                else
                {
                    context.Result = new NotFoundResult();
                }
            }
        }
    }

    public class ValidateFinancialResourceCategoryExistsAttribute : TypeFilterAttribute
    {
        public ValidateFinancialResourceCategoryExistsAttribute()
            : base(typeof(ValidateFinancialResourceCategoryExists))
        {
        }

        private class ValidateFinancialResourceCategoryExists : IAsyncActionFilter
        {
            private readonly IMerlinPlanRepository _repository;

            public ValidateFinancialResourceCategoryExists(IMerlinPlanRepository mprepo)
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
                        if (await _repository.FinancialResourceCategories.ToAsyncEnumerable().All(o => o.Id != id.Value))
                        {
                            context.Result = new NotFoundObjectResult(id.Value);
                            return;
                        }
                        await next();
                    }
                }
                else
                {
                    context.Result = new NotFoundResult();
                }
            }
        }
    }


    public class ValidateBusinessUnitExistsAttribute : TypeFilterAttribute
    {
        public ValidateBusinessUnitExistsAttribute() : base(typeof(ValidateBusinessUnitExists))
        {
        }

        private class ValidateBusinessUnitExists : IAsyncActionFilter
        {
            private readonly IMerlinPlanRepository _repository;

            public ValidateBusinessUnitExists(IMerlinPlanRepository mprepo)
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
                        if (await _repository.BusinessUnits.ToAsyncEnumerable().All(o => o.Id != id.Value))
                        {
                            context.Result = new NotFoundObjectResult(id.Value);
                            return;
                        }
                        await next();
                    }
                }
                else
                {
                    context.Result = new NotFoundResult();
                }
            }
        }
    }

    public class ValidateStaffResourceExistsAttribute : TypeFilterAttribute
    {
        public ValidateStaffResourceExistsAttribute() : base(typeof(ValidateStaffResourceExists))
        {
        }

        private class ValidateStaffResourceExists : IAsyncActionFilter
        {
            private readonly IMerlinPlanRepository _repository;

            public ValidateStaffResourceExists(IMerlinPlanRepository mprepo)
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
                        if (await _repository.StaffResources.ToAsyncEnumerable().All(o => o.Id != id.Value))
                        {
                            context.Result = new NotFoundObjectResult(id.Value);
                            return;
                        }
                        await next();
                    }
                }
                else
                {
                    context.Result = new NotFoundResult();
                }
            }
        }
    }


    public class ValidateUserExistsAttribute : TypeFilterAttribute
    {
        public ValidateUserExistsAttribute() : base(typeof(ValidateUserExists))
        {
        }

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
                        await next();
                    }
                }
                else
                {
                    context.Result = new NotFoundResult();
                }
            }
        }
    }
}

