using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using Microsoft.AspNetCore.Identity;
using MPWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using OpenIddict;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.Authentication;
using AspNet.Security.OpenIdConnect.Server;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MPWebAPI.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : Controller
    {
        private readonly OpenIddictUserManager<MerlinPlanUser> _userManager;
        private readonly SignInManager<MerlinPlanUser> _signInManager;
        private readonly ILogger _logger;
        
        public AuthController(
            OpenIddictUserManager<MerlinPlanUser> userManager, 
            SignInManager<MerlinPlanUser> signInManager,
            ILoggerFactory loggerFactory
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger("AuthController");
        }
        
        [HttpPost("token")]
        public async Task<IActionResult> Token()
        {
            var request = HttpContext.GetOpenIdConnectRequest();
            _logger.LogDebug(request.IsPasswordGrantType().ToString());
            if (request.IsPasswordGrantType())
            {
                // Check username and password validity
                var user = await _userManager.FindByNameAsync(request.Username);
                if (user == null)
                {
                    return BadRequest(new OpenIdConnectResponse {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "Username or password is incorrect."
                    } );
                }

                if (! await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    return BadRequest(new OpenIdConnectResponse {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "Username or password is incorrect."
                    });
                }

                // Check if the user can sign in
                if (!await _signInManager.CanSignInAsync(user)) {
                    return BadRequest(new OpenIdConnectResponse {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The specified user is not allowed to sign in."
                    });
                }

                // Ensure the user is not already locked out.
                if (_userManager.SupportsUserLockout && await _userManager.IsLockedOutAsync(user)) {
                    return BadRequest(new OpenIdConnectResponse {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "Username or password is incorrect."
                    });
                }

                if (_userManager.SupportsUserLockout) {
                    await _userManager.ResetAccessFailedCountAsync(user);
                }

                var identity = await _userManager.CreateIdentityAsync(user, request.GetScopes());

                // Create a new authentication ticket holding the user identity.
                var ticket = new AuthenticationTicket(
                    new ClaimsPrincipal(identity),
                    new AuthenticationProperties(),
                    OpenIdConnectServerDefaults.AuthenticationScheme);
                
                ticket.SetResources(request.GetResources());
                ticket.SetScopes(request.GetScopes());

                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }
            _logger.LogDebug("Did we get here?");
            return BadRequest(new OpenIdConnectResponse {
                Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
                ErrorDescription = "The specified grant type is not supported."
            });
        }
    }
}
