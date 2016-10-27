using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;
        
        public MerlinPlanBL(
            IOptions<MerlinPlanBLOptions> options, 
            IMerlinPlanRepository mprepo,
            ILoggerFactory loggerFactory
            )
        {
            _respository = mprepo;
            _options = options;
            _logger = loggerFactory.CreateLogger<MerlinPlanBL>();
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
        /// <param name="roles"></param>
        /// <returns></returns>
        public async Task<IdentityResult> CreateUser(MerlinPlanUser newUser, string password, IEnumerable<string> roles)
        {
            var roleList = roles.ToList();
            var org = _respository.Organisations.First(o => o.Id == newUser.OrganisationId);
            newUser.Organisation = org;
            var result = await _respository.CreateUserAsync(newUser, password);
            if (result.Succeeded)
            {
                // Add default role if none provided.
                var rolesToAdd = (roleList == null || !roleList.Any()) ? 
                    new List<string> {_options.Value.DefaultRole} : roleList;
                
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

        /// <summary>
        /// Only deletes a FRC if there are currently no Transactions or Partitions using it.
        /// </summary>
        /// <param name="frc"></param>
        /// <returns></returns>
        public async Task<MerlinPlanBLResult> DeleteFinancialResourceCategoryAsync(FinancialResourceCategory frc)
        {
            
            var result = new MerlinPlanBLResult();
            if (frc.FinancialPartitions.Count == 0 && frc.Transactions.Count == 0)
            {
                await _respository.RemoveFinancialResourceCategoryAsync(frc);
                return result;
            }
            if (frc.FinancialPartitions.Count > 0)
            {
                result.AddError(
                    "FinancialPartitions", 
                    $"Cannot delete {frc.Name} there are Financial Resource Partitions using this category.");    
            }

            if (frc.Transactions.Count > 0)
            {
                result.AddError(
                    "Transactions", 
                    $"Cannot delete {frc.Name} there are Financial Transactions using this category.");    
            }
            return result;
        }

        /// <summary>
        /// We need to check that the name of the category is unique for the group.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="categories"></param>
        /// <returns></returns>
        public async Task<MerlinPlanBLResult> AddFinancialResourceCategoriesAsync(
            Group group, 
            IEnumerable<FinancialResourceCategory> categories)
        {
            var categoryList = categories.ToList();
            var result = new MerlinPlanBLResult();
            foreach (var category in categoryList)
            {
                if (group.FinancialResourceCategories.Any(frc => frc.Name == category.Name))
                {
                    result.AddError("Name", $"The Financial Resource Category Name {category.Name} is already used in this group");
                }
            }

            if (result.Succeeded)
            {
                foreach (var category in categoryList)
                {
                    await _respository.AddFinancialResourceCategoryAsync(category);
                }
            }
            return result;
        }
        
        
        /// <summary>
        /// Adds a new Financial Resource to the specified Resource Scenario and
        /// creates a default partition for it.
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public async Task<MerlinPlanBLResult> AddFinancialResourceAsync(FinancialResource resource)
        {
            await _respository.AddFinancialResourceAsync(resource);
            // Add default partition

            var fp = new FinancialResourcePartition {FinancialResource = resource};
            await _respository.AddFinancialResourcePartitionAsync(fp);
            return new MerlinPlanBLResult();
        }

        public async Task<MerlinPlanBLResult> AddFinancialResourcePartitionsAsync(FinancialResource resource, IEnumerable<INewPartitionRequest> partitions)
        {
            var result = new MerlinPlanBLResult();
            
            // We need to check that partition does not already exist
            foreach (var newPartitionRequest in partitions)
            {
                // check for the default category
                if (newPartitionRequest.Categories == null || newPartitionRequest.Categories.Length == 0)
                {
                    result.AddError("Categories", "The defualt category already exists.");
                    return result;
                }

                var categoriesExist =
                    newPartitionRequest.Categories.All(
                        c => _respository.FinancialResourceCategories.Any(frc => frc.Name == c));

                if (categoriesExist)
                {
                    // for each partition in this resource:
                    // check that partition does not exist with those categories
                    foreach (var frp in resource.Partitions)
                    {
                        var pCatNames = frp.Categories.Select(frc => frc.FinancialResourceCategory.Name).ToList();
                        var hasAllCats = newPartitionRequest.Categories.All(c => pCatNames.Contains(c));
                        if (hasAllCats && (pCatNames.Count == newPartitionRequest.Categories.Length))
                        {
                            // we already have a partition with these categories, return a bad request
                            result.AddError("Categories",
                                $"A partition already exists with the categories {string.Join(", ", pCatNames)}");
                            return result;
                        }
                    }
                }

                // we can now safely add some new partitions
                var newPartition = new FinancialResourcePartition {FinancialResource = resource};
                await _respository.AddFinancialResourcePartitionAsync(newPartition);
                //_logger.LogDebug($"Created new partition with Id = {newPartition.Id}");
                
                // Add the categories, create if nessesary
                var npCategories = newPartitionRequest.Categories
                    .Select(category => _respository.FinancialResourceCategories.FirstOrDefault(frc => frc.Name == category) ?? 
                    new FinancialResourceCategory
                    {
                        Name = category,
                        Group = resource.ResourceScenario.Group
                    })
                    .ToList();

                npCategories.ForEach(async npc =>
                {
                    if (!_respository.FinancialResourceCategories.Contains(npc))
                    {
                        await _respository.AddFinancialResourceCategoryAsync(npc);
                    }
                });
                await _respository.AddCategoriesToFinancialPartitionAsync(newPartition, npCategories);

                // Add starting adjustment
                if (newPartitionRequest.StartingAdjustment == 0) continue;

                var newAdjustment = new FinancialAdjustment()
                {
                    Actual = false,
                    Additive = false,
                    Date = resource.StartDate,
                    FinancialResourcePartition = newPartition,
                    Value = newPartitionRequest.StartingAdjustment
                };

                await _respository.AddAdjustmentToFinancialResourceAsync(newAdjustment);
            }
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
                Errors.Add(key, new List<string>());
            }
            Errors[key].Add(error);
        }
        
        public bool Succeeded { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; }
    }    
}
