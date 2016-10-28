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
        Task CreateOrganisation(Organisation org);
        Task<IdentityResult> CreateUser(
            MerlinPlanUser newUser, 
            string password, 
            IEnumerable<string> roles
            );
        
        Task<MerlinPlanBLResult> ParentGroupAsync(Group child, Group parent);
        Task<MerlinPlanBLResult> UnparentGroupAsync(Group group);
        Task<MerlinPlanBLResult> DeleteFinancialResourceCategoryAsync(FinancialResourceCategory frc);
        Task<MerlinPlanBLResult> AddFinancialResourceAsync(FinancialResource resource);
        Task<MerlinPlanBLResult> AddFinancialResourceCategoriesAsync(Group group, IEnumerable<FinancialResourceCategory> categories);
        Task<MerlinPlanBLResult> AddFinancialResourcePartitionsAsync(FinancialResource resource, IEnumerable<INewPartitionRequest> partitions);
    }

    public interface INewPartitionRequest
    {
        string[] Categories { get; set; }
        decimal StartingAdjustment { get; set; }
        bool Actual { get; set; }
    }
}

