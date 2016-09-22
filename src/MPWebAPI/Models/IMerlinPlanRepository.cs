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
        IEnumerable<Organisation> Organisations { get; }
        Task AddOrganisationAsync(Organisation org);
        Task RemoveOrganisationAsync(Organisation org);
        IEnumerable<Group> GetOrganisationGroups(Organisation org);
        Task SaveChangesAsync();

        // Groups
        IEnumerable<Group> Groups { get; }
        Task AddGroupAsync(Group g);
        Task RemoveGroupAsync(Group g);
        Task<IEnumerable<MerlinPlanUser>> GetGroupMembersAsync(Group g);
    }    
}
