using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MPWebAPI.Models
{
    /// <summary>
    /// Concrete implementation of the Merlin Plan business logic
    /// </summary>
    public class MerlinPlanBL : IMerlinPlanBL
    {
        private readonly IMerlinPlanRepository _mprepo;
        private readonly UserManager<MerlinPlanUser> _userManager;
        
        public MerlinPlanBL(IMerlinPlanRepository mprepo, UserManager<MerlinPlanUser> userManager)
        {
            _mprepo = mprepo;
            _userManager = userManager;
        }
        
        /// <summary>
        /// Business logic for creating a new Org. We need to create a default
        /// group and a default admin user.
        /// </summary>
        /// <param name="org">The organisation model to add</param>
        /// <returns></returns>
        public async Task CreateOrganisation(Organisation org)
        {
            await _mprepo.AddOrganisationAsync(org);
            // TODO: Add default user
            // TODO: Add default group
        }

        /// <summary>
        /// Business logic for creating a new user. Adds the user to the correct 
        /// group and organisation and in future might send out a registration email.
        /// </summary>
        /// <param name="newUser">the new user to add</param>
        /// <param name="password">the user's password</param>
        /// <returns></returns>
        public async Task<IdentityResult> CreateUser(MerlinPlanUser newUser, string password, IEnumerable<string> roles)
        {
            var org = _mprepo.Organisations.First(o => o.Id == newUser.OrganisationId);
            newUser.Organisation = org;
            var result = await _userManager.CreateAsync(newUser, password);
            if (result.Succeeded)
            {
                // Add user to groups
                var roleAddResult = await _userManager.AddToRolesAsync(newUser, roles);
                if (!roleAddResult.Succeeded)
                {
                    return roleAddResult;
                }
            }
            return result;
        }
    }    
}
