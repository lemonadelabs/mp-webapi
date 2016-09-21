using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MPWebAPI.Models
{
    /// <summary>
    /// Encapsulates the business logic for the Merlin: Plan application.
    /// </summary>
    public interface IMerlinPlanBL
    {
        Task CreateOrganisation(Organisation org);
        Task<IdentityResult> CreateUser(MerlinPlanUser newUser, string password, IEnumerable<string> roles);
    }    
}

