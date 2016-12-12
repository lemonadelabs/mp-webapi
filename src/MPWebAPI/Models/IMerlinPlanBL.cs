using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MPWebAPI.ViewModels;

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
        Task<MerlinPlanBLResult> AddFinancialResourceAsync(FinancialResource resource, decimal? defaultPartitionValue = null);
        Task<MerlinPlanBLResult> AddFinancialResourceCategoriesAsync(Group group, IEnumerable<FinancialResourceCategory> categories);
        Task<MerlinPlanBLResult> AddFinancialResourcePartitionsAsync(FinancialResource resource, IEnumerable<IPartitionRequest> partitions);
        Task<MerlinPlanBLResult> UpdateFinancialResourceAsync(FinancialResource resource);
        Task<MerlinPlanBLResult> DeleteFinancialResourcePartitionAsync(FinancialResourcePartition partition);
        Task<MerlinPlanBLResult> UpdateFinancialResourcePartitionsAsync(FinancialResource resource, IEnumerable<IPartitionUpdate> partitions);
        Task<MerlinPlanBLResult> CopyFinancialResourcesAsync(IEnumerable<IResourceCopyRequest> requests);
        Task<MerlinPlanBLResult> CopyResourceScenariosAsync(IEnumerable<IDocumentCopyRequest> requests);
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
        Task<MerlinPlanBLResult> CopyProjectAsync(IEnumerable<IDocumentCopyRequest> requests);
        Task<MerlinPlanBLResult> UpdateProjectAsync(IEnumerable<IProjectUpdate> requests);
        #endregion

        #region Project Options
        Task<MerlinPlanBLResult> AddProjectPhaseAsync(ProjectPhase phase);
        Task<MerlinPlanBLResult> DeleteProjectPhaseAsync(ProjectPhase phase);
        Task<MerlinPlanBLResult> DeleteProjectOptionAsync(ProjectOption option);
        Task<MerlinPlanBLResult> AddBenefitCategoriesAsync(Group group, IEnumerable<BenefitCategory> categories);
        Task<MerlinPlanBLResult> DeleteBenefitCategoryAsync(BenefitCategory category);
        Task<MerlinPlanBLResult> AddProjectBenefitAsync(ProjectBenefit benefit);
        Task<MerlinPlanBLResult> DeleteProjectBenefitAsync(ProjectBenefit benefit);
        Task<MerlinPlanBLResult> DeleteAlignmentCategoryAsync(AlignmentCategory category);
        Task<MerlinPlanBLResult> DeleteRiskCategoryAsync(RiskCategory category);
        #endregion

        #region Portfolios
        Task<MerlinPlanBLResult> DeletePortfolioAsync(Portfolio portfolio);
        Task<MerlinPlanBLResult> AddPortfolioAsync(Portfolio portfolio);
        Task<MerlinPlanBLResult> UpdatePortfolioAsync(IEnumerable<IPortfolioUpdate> requests);
        Task<MerlinPlanBLResult> AddProjectToPortfolioAsync(Portfolio portfolio, IEnumerable<IAddProjectToPortfolioRequest> requests);
        Task<MerlinPlanBLResult> RemoveProjectFromPortfolioAsync(ProjectConfig projectConfig);
        Task<MerlinPlanBLResult> UpdatePortfolioProjectAsync(ProjectConfig projectConfig);

        #endregion

    }

    #region Data Object Interfaces

    public interface IAddProjectToPortfolioRequest
    {
        int ProjectId { get; set; }
        string[] Tags { get; set; }
        DateTime? EstimatedStartDate { get; set; }
        int? Owner { get; set; }
        int[] Managers { get; set; }
        int OptionId { get; set; }
    }

    public interface IPortfolioUpdate
    {
        int Id { get; set; }
        string Name { get; set; }
        DateTime? StartYear { get; set; }
        DateTime? EndYear { get; set; }
        int? TimeScale { get; set; }
    }

    public interface IProjectUpdate
    {
        int Id { get; set; }
        string Name { get; set; }
        string Summary { get; set; }
        string Reference { get; set; }
        string[] Categories { get; set; }
        string OwningBusinessUnit { get; set; }
        string ImpactedBusinessUnit { get; set; }
    }

    public interface IPartitionUpdate
    {
        int Id { get; set; }
        string[] Categories { get; set; }
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

    public interface IDocumentCopyRequest
    {
        int Id { get; set; }
        int Group { get; set; }
        string Name { get; set; }
        string User { get; set; }
    }

    #endregion
}

