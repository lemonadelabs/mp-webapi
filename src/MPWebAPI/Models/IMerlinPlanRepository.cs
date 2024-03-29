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
        #region Organisations

        IEnumerable<Organisation> Organisations { get; }
        Task AddOrganisationAsync(Organisation org);
        Task RemoveOrganisationAsync(Organisation org);
        IEnumerable<Group> GetOrganisationGroups(Organisation org);

        #endregion

        // Users
        #region Users

        IEnumerable<MerlinPlanUser> Users { get; }
        Task<MerlinPlanUser> FindUserByUserNameAsync(string userName);
        Task<IEnumerable<string>> GetUserRolesAsync(MerlinPlanUser user);
        Task<IdentityResult> UpdateUserAsync(MerlinPlanUser user);
        Task<IdentityResult> RemoveUserFromRolesAsync(MerlinPlanUser userm, IEnumerable<string> rolesToDelete);
        Task<IdentityResult> AddUserToRolesAsync(MerlinPlanUser userm, IEnumerable<string> rolesToAdd);
        Task<IEnumerable<Group>> GetUserGroupsAsync(MerlinPlanUser user);
        Task<IdentityResult> CreateUserAsync(MerlinPlanUser user, string password);

        #endregion

        // Groups
        #region Groups

        IEnumerable<Group> Groups { get; }
        Task AddGroupAsync(Group group);
        Task RemoveGroupAsync(Group group);
        Task<IEnumerable<MerlinPlanUser>> GetGroupMembersAsync(Group group);
        Task AddUserToGroupAsync(MerlinPlanUser user, Group group);
        Task RemoveUserFromGroupAsync(MerlinPlanUser user, Group group);
        Task ParentGroupAsync(Group child, Group parent);
        Task UnparentGroupAsync(Group group);
        Task GroupSetActive(Group g, bool active);

        #endregion

        // Resource Scenarios
        #region Resource Scenarios

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

        #endregion

        #region Portfolio

        IEnumerable<Portfolio> Portfolios { get; }
        Task<IEnumerable<Portfolio>> GetUserSharedPortfoliosForUserAsync(MerlinPlanUser user);
        Task<IEnumerable<Portfolio>> GetGroupSharedPortfoliosForUserAsync(MerlinPlanUser user);
        Task<IEnumerable<Portfolio>> GetOrganisationSharedPortfoliosAsync(Organisation org);
        Task SharePortfolioWithGroupAsync(Portfolio portfolio, bool share);
        Task SharePortfolioWithOrgAsync(Portfolio portfolio, bool share);
        Task SharePortfolioWithUserAsync(Portfolio portfolio, MerlinPlanUser user);
        Task UnsharePortfolioWithUserAsync(Portfolio portfolio, MerlinPlanUser user);
        Task AddPortfolioAsync(Portfolio portfolio);
        Task RemovePortfolioAsync(Portfolio portfolio);


        #endregion

        #region Project Config

        IEnumerable<ProjectConfig> ProjectConfigs { get; }
        Task AddProjectConfigAsync(ProjectConfig config);
        Task RemoveProjectConfigAsync(ProjectConfig config);
        Task AddTagToPortfolioAsync(Portfolio portfolio, string tag);
        Task RemoveTagFromPortfolioAsync(Portfolio portfolio, string tag);
        Task AddTagsToProjectConfigAsync(ProjectConfig projectConfig, IEnumerable<string> tags);
        Task RemoveTagsFromProjectConfigAsync(ProjectConfig projectConfig, IEnumerable<string> tags);
        Task AddManagersToProjectConfigAsync(ProjectConfig projectConfig, IEnumerable<int> staffResources);
        Task RemoveManagersFromProjectConfigAsync(ProjectConfig projectConfig, IEnumerable<int> staffResources);

        #endregion

        // Financial Resources
        #region Financial Resources

        Task AddFinancialResourceAsync(FinancialResource resource);
        Task RemoveFinancialResourceAsync(FinancialResource resource);
        IEnumerable<FinancialResource> FinancialResources { get; }

        #endregion

        #region Financial Resource Partition

        IEnumerable<FinancialResourcePartition> FinancialResourcePartitions { get; }
        Task AddFinancialResourcePartitionAsync(FinancialResourcePartition partition);
        Task RemoveFinancialResourcePartitionAsync(FinancialResourcePartition partition);

        Task AddCategoriesToFinancialPartitionAsync(FinancialResourcePartition partition,
            IEnumerable<FinancialResourceCategory> categories);

        Task RemoveCategoriesFromFinancialPartitionAsync(FinancialResourcePartition partition,
            IEnumerable<FinancialResourceCategory> categories);

        Task AddAdjustmentToFinancialResourceAsync(FinancialAdjustment adjustment);


        #endregion

        // Staff Resources
        #region Staff Resource

        Task AddStaffResourceAsync(StaffResource resource);
        Task RemoveStaffResourceAsync(StaffResource resource);
        IEnumerable<StaffResource> StaffResources { get; }
        Task AddCategoriesToStaffResourceAsync(IEnumerable<StaffResourceCategory> categories, StaffResource resource);
        Task RemoveCategoriesFromStaffResourceAsync(IEnumerable<StaffResourceCategory> categories,
            StaffResource resource);

        #endregion

        // Categories
        #region Financial Resource Categories

        IEnumerable<FinancialResourceCategory> FinancialResourceCategories { get; }
        Task AddFinancialResourceCategoryAsync(FinancialResourceCategory category);
        Task RemoveFinancialResourceCategoryAsync(FinancialResourceCategory category);


        #endregion

        #region Staff Resource Categories

        IEnumerable<StaffResourceCategory> StaffResourceCategories { get; }
        Task AddStaffResourceCategoryAsync(StaffResourceCategory category);
        Task RemoveStaffResourceCategoryAsync(StaffResourceCategory category);

        #endregion

        #region Business Units

        IEnumerable<BusinessUnit> BusinessUnits { get; }
        Task AddBusinessUnitAsync(BusinessUnit businessUnit);
        Task RemoveBusinessUnitAsync(BusinessUnit businessUnit);

        #endregion

        #region Benefit Categories

        IEnumerable<BenefitCategory> BenefitCategories { get; }
        Task AddBenefitCategoryAsync(BenefitCategory category);
        Task RemoveBenefitCategoryAsync(BenefitCategory category);

        #endregion

        #region Alignment Categories

        IEnumerable<AlignmentCategory> AlignmentCategories { get; }
        Task AddAlignmentCategoryAsync(AlignmentCategory category);
        Task RemoveAlignmentCategoryAsync(AlignmentCategory category);

        #endregion

        #region Risk Categories

        IEnumerable<RiskCategory> RiskCategories { get; }
        Task AddRiskCategoryAsync(RiskCategory category);
        Task RemoveRiskCategoryAsync(RiskCategory category);

        #endregion

        #region Project

        IEnumerable<Project> Projects { get; }
        IEnumerable<Project> GetUserSharedProjectsForUser(MerlinPlanUser user);
        IEnumerable<Project> GetGroupShareProjectsForUser(MerlinPlanUser user);
        IEnumerable<Project> GetOrganisationSharedProjects(Organisation org);
        Task ShareProjectWithGroupAsync(Project project, bool share);
        Task ShareProjectWithOrgAsync(Project project, bool share);
        Task ShareProjectWithUserAsync(Project project, MerlinPlanUser user);
        Task UnshareProjectWithUserAsync(Project project, MerlinPlanUser user);
        Task AddProjectAsync(Project project);
        Task RemoveProjectAsync(Project project);

        Task AddFinancialResourceCategoriesToProjectAsync(Project project,
            IEnumerable<FinancialResourceCategory> categories);

        #endregion

        #region Project Option

        IEnumerable<ProjectOption> ProjectOptions { get; }
        Task AddProjectOptionAsync(ProjectOption option);
        Task AddProjectDependencyAsync(ProjectOption source, ProjectOption target);
        Task RemoveProjectDependencyAsync(ProjectOption option, ProjectOption target);
        Task AddProjectBenefitAsync(ProjectBenefit benefit);
        Task RemoveProjectBenefitAsync(ProjectBenefit benefit);
        Task RemoveProjectOptionAsync(ProjectOption option);
        IEnumerable<ProjectDependency> ProjectDependencies { get; }

            #endregion

        #region Risk Profile

        IEnumerable<RiskProfile> RiskProfiles { get; }
        Task AddRiskProfileAsync(RiskProfile profile);
        Task RemoveRiskProfileAsync(RiskProfile profile);

        #endregion

        #region Project Phase

        IEnumerable<ProjectPhase> ProjectPhases { get; }
        Task AddProjectPhaseAsync(ProjectPhase phase);
        Task RemoveProjectPhaseAsync(ProjectPhase phase);

        #endregion

        #region Project Benefit

        IEnumerable<ProjectBenefit> ProjectBenefits { get; }

        #endregion

        #region Alignment

        IEnumerable<Alignment> Alignments { get; }
        Task RemoveAlignmentAsync(Alignment alignment);
        Task AddAlignmentAsync(Alignment alignment);

        #endregion



        Task SaveChangesAsync();
    }
}
