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
        #region Organisations

        Task CreateOrganisation(Organisation org);

        #endregion

        #region Users

        Task<IdentityResult> CreateUser(
            MerlinPlanUser newUser, 
            string password, 
            IEnumerable<string> roles
            );

        #endregion

        #region Groups

        Task<MerlinPlanBLResult> ParentGroupAsync(Group child, Group parent);
        Task<MerlinPlanBLResult> UnparentGroupAsync(Group group);

        #endregion

        #region Financial Resources

        Task<MerlinPlanBLResult> DeleteFinancialResourceCategoryAsync(FinancialResourceCategory frc);
        Task<MerlinPlanBLResult> AddFinancialResourceAsync(FinancialResource resource);
        Task<MerlinPlanBLResult> AddFinancialResourceCategoriesAsync(Group group, IEnumerable<FinancialResourceCategory> categories);
        Task<MerlinPlanBLResult> AddFinancialResourcePartitionsAsync(FinancialResource resource, IEnumerable<IPartitionRequest> partitions);
        Task<MerlinPlanBLResult> UpdateFinancialResourceAsync(FinancialResource resource);
        Task<MerlinPlanBLResult> DeleteFinancialResourcePartitionAsync(FinancialResourcePartition partition);
        Task<MerlinPlanBLResult> UpdateFinancialResourcePartitionsAsync(FinancialResource resource, IEnumerable<IPartitionUpdate> partitions);
        Task<MerlinPlanBLResult> CopyFinancialResourcesAsync(IEnumerable<IResourceCopyRequest> requests);
        Task<MerlinPlanBLResult> CopyResourceScenariosAsync(IEnumerable<IScenarioCopyRequest> requests);

        #endregion

        #region Staff Resources

        Task<MerlinPlanBLResult> UpdateStaffResourceAsync(StaffResource resource);
        Task<MerlinPlanBLResult> CopyStaffResourcesAsync(IEnumerable<IResourceCopyRequest> requests);

        #endregion

        #region Business Units

        Task<MerlinPlanBLResult> AddBusinessUnitAsync(BusinessUnit businessUnit);
        Task<MerlinPlanBLResult> DeleteBusinessUnitAsync(BusinessUnit businessUnit);

        #endregion

        #region Projects

        Task<MerlinPlanBLResult> DeleteProjectAsync(Project project);
        Task<MerlinPlanBLResult> AddProjectAsync(Project project);

        #endregion

    }

    #region Data Object Interfaces

    public interface IPartitionUpdate
    {
        int Id { get; set; }
        decimal Adjustment { get; set; }
        bool Actual { get; set; }
    } 

    public interface IPartitionRequest
    {
        string[] Categories { get; set; }
        decimal Adjustment { get; set; }
        bool Actual { get; set; }
    }

    public interface IResourceCopyRequest
    {
        int Id { get; set; }
        int ResourceScenario { get; set; }
        string Name { get; set; }
    }

    public interface IScenarioCopyRequest
    {
        int Id { get; set; }
        int Group { get; set; }
        string Name { get; set; }
        string User { get; set; }
    }

    #endregion
}

