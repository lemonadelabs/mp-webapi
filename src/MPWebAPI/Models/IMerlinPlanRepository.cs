using System.Collections.Generic;

namespace MPWebAPI.Models
{
    /// <summary>
    /// Represents the persistence store for Merlin: Plan 
    /// </summary>
    public interface IMerlinPlanRepository
    {
        // Organisations
        IEnumerable<Organisation> Organisations { get;}
        void AddOrganisation(Organisation org);
        void RemoveOrganisation(int orgId);
        void AddOrganisations(IEnumerable<Organisation> orgs);
        void RemoveOrganisations(IEnumerable<int> orgIds);
        void UpdateOrganisation(Organisation org);
    }    
}
