using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace MPWebAPI.Models
{
    
    // Config options for MerlinPlanBL
    public class MerlinPlanBLOptions
    {
        public string DefaultRole { get; set; }
    }
    
    /// <summary>
    /// Concrete implementation of the Merlin Plan business logic
    /// </summary>
    public class MerlinPlanBL : IMerlinPlanBL
    {
        private readonly IMerlinPlanRepository _respository;
        private readonly IOptions<MerlinPlanBLOptions> _options;
        
        public MerlinPlanBL(
            IOptions<MerlinPlanBLOptions> options, 
            IMerlinPlanRepository mprepo
            )
        {
            _respository = mprepo;
            _options = options;
        }
        
        /// <summary>
        /// Business logic for creating a new Org. We need to create a default
        /// group and a default admin user.
        /// </summary>
        /// <param name="org">The organisation model to add</param>
        /// <returns></returns>
        public async Task CreateOrganisation(Organisation org)
        {
            await _respository.AddOrganisationAsync(org);
            var orgGroup = new Group {
                Name = org.Name,
                Organisation = org
            };
            await _respository.AddGroupAsync(orgGroup);
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
            var org = _respository.Organisations.First(o => o.Id == newUser.OrganisationId);
            newUser.Organisation = org;
            var result = await _respository.CreateUserAsync(newUser, password);
            if (result.Succeeded)
            {
                // Add default role if none provided.
                var rolesToAdd = (roles == null || roles.Count() == 0) ? 
                    new List<string> {_options.Value.DefaultRole} : roles;
                
                // Add user to roles
                var roleAddResult = await _respository.AddUserToRolesAsync(newUser, rolesToAdd);
                if (!roleAddResult.Succeeded)
                {
                    return roleAddResult;
                }
            }
            return result;
        }

        public async Task<MerlinPlanBLResult> ParentGroupAsync(Group child, Group parent)
        {
            var result = new MerlinPlanBLResult();
            
            // check if groups are the same
            if (child.Id == parent.Id)
            {
                result.AddError("ChildId", "A group cannot be a parent of itself.");
                return result;
            }

            // check that parent is not a child of child (circular grouping)
            var cGroup = parent;
            while (cGroup.Parent != null)
            {
                if (cGroup.Parent.Id == child.Id)
                {
                    result.AddError("ParentId", "The parent cannot be a child of the child");
                    return result;
                }
                cGroup = cGroup.Parent;
            }

            // check both groups belong to the same org
            if (child.OrganisationId != parent.OrganisationId)
            {
                result.AddError("OrganisationId", "Both groups need to belong to the same organisation.");
                return result;
            }

            // Do parenting
            await _respository.ParentGroupAsync(child, parent);
            return result; 
        }

        public async Task<MerlinPlanBLResult> UnparentGroupAsync(Group group)
        {
            var result = new MerlinPlanBLResult();
            await _respository.UnparentGroupAsync(group);
            return result;
        }
    }

    

    public class MerlinPlanBLResult
    {
        public MerlinPlanBLResult()
        {
            Succeeded = true;
            Errors = new Dictionary<string, List<string>>();
        }

        public void AddError(string key, string error)
        {
            Succeeded = false;
            if (!Errors.ContainsKey(key))
            {
                Errors.Add(key, new List<string> {error});
            }
            Errors[key].Add(error);
        }
        
        public bool Succeeded { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; }
    }    
}
