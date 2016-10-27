using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

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

        // Users
        IEnumerable<MerlinPlanUser> Users { get; }
        Task<MerlinPlanUser> FindUserByUserNameAsync(string userName);
        Task<IEnumerable<string>> GetUserRolesAsync(MerlinPlanUser user);
        Task<IdentityResult> UpdateUserAsync(MerlinPlanUser user);
        Task<IdentityResult> RemoveUserFromRolesAsync(MerlinPlanUser userm, IEnumerable<string> rolesToDelete);
        Task<IdentityResult> AddUserToRolesAsync(MerlinPlanUser userm, IEnumerable<string> rolesToAdd);
        Task<IEnumerable<Group>> GetUserGroupsAsync(MerlinPlanUser user);
        Task<IdentityResult> CreateUserAsync(MerlinPlanUser user, string password);

        // Groups
        IEnumerable<Group> Groups { get; }
        Task AddGroupAsync(Group group);
        Task RemoveGroupAsync(Group group);
        Task<IEnumerable<MerlinPlanUser>> GetGroupMembersAsync(Group group);
        Task AddUserToGroupAsync(MerlinPlanUser user, Group group);
        Task RemoveUserFromGroupAsync(MerlinPlanUser user, Group group);
        Task ParentGroupAsync(Group child, Group parent);
        Task UnparentGroupAsync(Group group);
        Task GroupSetActive(Group g, bool active);

        // Resource Scenarios
        IEnumerable<ResourceScenario> ResourceScenarios { get; }
        Task<IEnumerable<ResourceScenario>> GetUserSharedResourceScenariosForUserAsync(MerlinPlanUser user);
        Task<IEnumerable<ResourceScenario>> GetGroupSharedResourceScenariosForUserAsync(MerlinPlanUser user);
        Task<IEnumerable<ResourceScenario>> GetOrganisationSharedResourceScenariosAsync(Organisation org);
        Task ShareResourceScenarioWithGroupAsync(ResourceScenario scenario, bool share);
        Task ShareResourceScenarioWithOrgAsync(ResourceScenario scenario, bool share);
        Task ShareResourceScenarioWithUserAsync(ResourceScenario scenario, MerlinPlanUser user);
        Task UnshareResourceScenarioWithUserAsync(ResourceScenario scenario, MerlinPlanUser user);
        Task AddResourceScenarioAsync(ResourceScenario scenario);
        Task RemoveResourceScenarioAsync(ResourceScenario scenario);

        // Financial Resources
        Task AddFinancialResourceAsync(FinancialResource resource);
        Task RemoveFinancialResourceAsync(FinancialResource resource);
        IEnumerable<FinancialResource> FinancialResources { get; }
        IEnumerable<FinancialResourcePartition> FinancialResourcePartitions { get; }
        Task AddFinancialResourcePartitionAsync(FinancialResourcePartition partition);
        Task RemoveFinancialResourcePartitionAsync(FinancialResourcePartition partition);

        Task AddCategoriesToFinancialPartitionAsync(FinancialResourcePartition partition,
            IEnumerable<FinancialResourceCategory> categories);

        Task RemoveCategoriesFromFinancialPartitionAsync(FinancialResourcePartition partition,
            IEnumerable<FinancialResourceCategory> categories);

        Task AddAdjustmentToFinancialResourceAsync(FinancialAdjustment adjustment);

        // Staff Resources
        Task AddStaffResourceAsync(StaffResource resource);
        Task RemoveStaffResourceAsync(StaffResource resource);
        IEnumerable<StaffResource> StaffResources { get; }

        // Categories
        IEnumerable<FinancialResourceCategory> FinancialResourceCategories { get; }
        Task AddFinancialResourceCategoryAsync(FinancialResourceCategory category);
        Task RemoveFinancialResourceCategoryAsync(FinancialResourceCategory category);
    }    
}
