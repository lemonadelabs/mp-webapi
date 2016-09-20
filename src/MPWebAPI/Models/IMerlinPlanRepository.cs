using System.Collections.Generic;
using System.Threading.Tasks;

namespace MPWebAPI.Models
{
    /// <summary>
    /// Represents the persistence store for Merlin: Plan 
    /// </summary>
    public interface IMerlinPlanRepository
    {
        // Organisations
        IEnumerable<Organisation> Organisations { get;}
        Task AddOrganisation(Organisation org);
        Task RemoveOrganisation(Organisation org);
        Task SaveChanges();
    }    
}
