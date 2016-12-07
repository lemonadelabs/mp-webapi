using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Identity;
using MPWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;
using AspNet.Security.OpenIdConnect.Server;


namespace MPWebAPI.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<MerlinPlanUser> _userManager;
        private readonly SignInManager<MerlinPlanUser> _signInManager;
        //private readonly ILogger _logger;
        
        public AuthController(
            UserManager<MerlinPlanUser> userManager, 
            SignInManager<MerlinPlanUser> signInManager/*,
            ILoggerFactory loggerFactory*/
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            //_logger = loggerFactory.CreateLogger("AuthController");
        }
        
        [HttpPost("token")]
        [Produces("application/json")]
        public async Task<IActionResult> Token(OpenIdConnectRequest request)
        {
            
            //var request = HttpContext.GetOpenIdConnectRequest();
            if (!request.IsPasswordGrantType())
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
                    ErrorDescription = "The specified grant type is not supported."
                });
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

            // If the user's email is not confirmed then they can't log in
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return BadRequest(new OpenIdConnectResponse {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "The user's email has not been confirmed."
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

            // Create a new authentication ticket holding the user identity.
            var ticket = await CreateTicketAsync(user);

            return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
        }

        private async Task<AuthenticationTicket> CreateTicketAsync(MerlinPlanUser user)
        {
            var principal = await _signInManager.CreateUserPrincipalAsync(user);

            foreach (var principalClaim in principal.Claims)
            {
                principalClaim.SetDestinations(OpenIdConnectConstants.Destinations.AccessToken,
                    OpenIdConnectConstants.Destinations.IdentityToken);
            }

            var ticket = new AuthenticationTicket(
                    principal, new AuthenticationProperties(),
                    OpenIdConnectServerDefaults.AuthenticationScheme 
                );

            ticket.SetScopes(
                OpenIdConnectConstants.Scopes.OpenId,
                OpenIdConnectConstants.Scopes.Email,
                OpenIdConnectConstants.Scopes.Profile,
                OpenIdConnectConstants.Scopes.OfflineAccess,
                "roles"
            );

            return ticket;
        }
    }
}
