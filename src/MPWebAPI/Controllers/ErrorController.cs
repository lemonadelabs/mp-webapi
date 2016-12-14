using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MPWebAPI.Controllers
{
    public class ErrorController : ControllerBase
    {
        [Route("/api/error")]
        public IActionResult HandleError()
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var error = feature?.Error;
            if (HttpContext.Response.StatusCode == 500)
            {
                return new JsonResult($"(500) Oh noes! Your request could not be served because things went explody!\n\n Stack Trace: {error?.StackTrace}");
            }
            return StatusCode(HttpContext.Response.StatusCode);
        }
    }
}