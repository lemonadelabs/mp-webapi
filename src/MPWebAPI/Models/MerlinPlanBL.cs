using System;
using System.Threading.Tasks;

namespace MPWebAPI.Models
{
    /// <summary>
    /// Concrete implementation of the Merlin Plan business logic
    /// </summary>
    public class MerlinPlanBL : IMerlinPlanBL
    {
        private readonly IMerlinPlanRepository _mprepo;
        
        public MerlinPlanBL(IMerlinPlanRepository mprepo)
        {
            _mprepo = mprepo;
        }
        
        /// <summary>
        /// Business logic for creating a new Org. We need to create a default
        /// group and a default admin user.
        /// </summary>
        /// <param name="org"></param>
        /// <returns></returns>
        public async Task CreateOrganisation(Organisation org)
        {
            await _mprepo.AddOrganisation(org);
            // TODO: Add default user
            // TODO: Add default group
        }
    }    
}
