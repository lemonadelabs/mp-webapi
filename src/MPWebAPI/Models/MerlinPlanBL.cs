using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace MPWebAPI.Models
{
    
    // Config options for MerlinPlanBL
    public class MerlinPlanBLOptions
    {
        public string DefaultRole { get; set; }
    }

    public class MerlinPlanBLResult
    {
        private object _data;

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

        public T GetData<T>() where T: class
        {
            return _data as T;
        }

        public void SetData(object data)
        {
            _data = data;
        }
        
        public bool Succeeded { get; set; }
        
        public Dictionary<string, List<string>> Errors { get; set; }
    }

    public class CopyRequest : IResourceCopyRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int ResourceScenario { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// Concrete implementation of the Merlin Plan business logic
    /// </summary>
    public class MerlinPlanBL : IMerlinPlanBL
    {
        private readonly IMerlinPlanRepository _respository;
        private readonly IOptions<MerlinPlanBLOptions> _options;

        public MerlinPlanBL(
            IOptions<MerlinPlanBLOptions> options, 
            IMerlinPlanRepository mprepo
            )
        {
            _respository = mprepo;
            _options = options;
        }

        #region Organisations

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

        #endregion

        #region Users

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

        #endregion

        #region Groups

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

        #endregion

        #region Financial Resources

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

            if (!result.Succeeded) return result;
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
        /// <param name="defaultPartitionValue"></param>
        /// <returns></returns>
        public async Task<MerlinPlanBLResult> AddFinancialResourceAsync(FinancialResource resource, decimal? defaultPartitionValue = null)
        {
            await _respository.AddFinancialResourceAsync(resource);
            // Add default partition

            var fp = new FinancialResourcePartition {FinancialResource = resource};
            await _respository.AddFinancialResourcePartitionAsync(fp);

            var partition = _respository.FinancialResourcePartitions.Single(p => p.Id == fp.Id);

            // Add the start and end adjustments for the default partition
            var startAjustment = new FinancialAdjustment()
            {
                FinancialResourcePartition = fp,
                Date = resource.StartDate,
                Value = defaultPartitionValue ?? 0,
                Actual = false,
                Additive = false,
            };

            partition.Adjustments.Add(startAjustment);

            if (resource.EndDate.HasValue)
            {
                var endAdjustment = new FinancialAdjustment()
                {
                    FinancialResourcePartition = fp,
                    Date = resource.EndDate.Value,
                    Value = 0,
                    Actual = false,
                    Additive = false
                };
                partition.Adjustments.Add(endAdjustment);
            }

            await _respository.SaveChangesAsync();
            return new MerlinPlanBLResult();
        }

        public async Task<MerlinPlanBLResult>  AddFinancialResourcePartitionsAsync(FinancialResource resource, IEnumerable<IPartitionRequest> partitions)
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

                foreach (var category in npCategories)
                {
                    if (_respository.FinancialResourceCategories.Contains(category)) continue;
                    await _respository.AddFinancialResourceCategoryAsync(category);
                }

                await _respository.AddCategoriesToFinancialPartitionAsync(newPartition, npCategories);

                // Add starting adjustment
                if (newPartitionRequest.Adjustment == 0) continue;

                var newStartAdjustment = new FinancialAdjustment()
                {
                    Actual = newPartitionRequest.Actual,
                    Additive = false,
                    Date = resource.StartDate,
                    FinancialResourcePartition = newPartition,
                    Value = newPartitionRequest.Adjustment
                };

                await _respository.AddAdjustmentToFinancialResourceAsync(newStartAdjustment);

                // Add ending adjustment if the financial resource has an end date
                if (resource.EndDate == null) continue;

                var newEndAdjustment = new FinancialAdjustment()
                {
                    Actual = newPartitionRequest.Actual,
                    Additive = false,
                    Date = (DateTime) resource.EndDate,
                    FinancialResourcePartition = newPartition,
                    Value = 0
                };

                await _respository.AddAdjustmentToFinancialResourceAsync(newEndAdjustment);
            }
            return result;
        }

        public async Task<MerlinPlanBLResult> UpdateFinancialResourceAsync(FinancialResource resource)
        {
            var result = new MerlinPlanBLResult();
            
            // we need to check and see if the resource scenario is locked.
            // If it's locked, then we cant edit the financial resource.
            if (resource.ResourceScenario.Approved)
            {
                result.AddError("Approved", "The resource belongs to an approved scenario and cannot be edited.");
                return result;
            }

            foreach (var partition in resource.Partitions.ToList())
            {
                // Handle updating the start date
                var first = partition.Adjustments.OrderBy(a => a.Date).FirstOrDefault();
                var last = partition.Adjustments.Count >= 2
                    ? partition.Adjustments.OrderByDescending(a => a.Date).FirstOrDefault()
                    : null;

                if (first != null && first.Date != resource.StartDate)
                {
                    first.Date = resource.StartDate;
                }

                // Handle adding or nulling the enddate
                if (
                    resource.EndDate.HasValue &&
                    resource.EndDate != resource.StartDate &&
                    partition.Adjustments.Count <= 1
                    )
                {
                    var newAdjustment = new FinancialAdjustment
                    {
                        FinancialResourcePartition = partition,
                        Date = resource.EndDate.Value,
                        Actual = false,
                        Additive = false,
                        Value = 0
                    };
                    partition.Adjustments.Add(newAdjustment);
                }
                else if (!resource.EndDate.HasValue && partition.Adjustments.Count >= 2)
                {
                    // We need to remove the end transaction as the enddate has been nullified.
                    if (last != null)
                    {
                        partition.Adjustments.Remove(last);
                    }
                }
                else if(resource.EndDate.HasValue && partition.Adjustments.Count >= 2)
                {
                    if (last != null && last.Date != resource.EndDate.Value)
                    {
                        last.Date = resource.EndDate.Value;
                    }
                }
            }

            await _respository.SaveChangesAsync();
            return result;
        }

        public async Task<MerlinPlanBLResult> DeleteFinancialResourcePartitionAsync(FinancialResourcePartition partition)
        {
            var result = new MerlinPlanBLResult();
            
            // If the resource scenario is approved, we have to forbid this
            if (partition.FinancialResource.ResourceScenario.Approved)
            {
                result.AddError("Approved", "The resource scenario is approved so this partition cannot be removed.");
                return result;
            }
            await _respository.RemoveFinancialResourcePartitionAsync(partition);
            return result;
        }

        public async Task<MerlinPlanBLResult> UpdateFinancialResourcePartitionsAsync(FinancialResource resource, IEnumerable<IPartitionUpdate> partitions)
        {
            var result = new MerlinPlanBLResult();
            var ps = partitions.ToList();
           
            foreach (var partition in ps)
            {
                // First validate that the partitions exist
                if (_respository.FinancialResourcePartitions.FirstOrDefault(frp => frp.Id == partition.Id) == null)
                {
                    result.AddError("Id", $"The a partition with id {partition.Id} could not be found");
                }

                // Check that we are allowed to add actuals
                if (partition.Actual && !resource.ResourceScenario.Approved)
                {
                    result.AddError("Actual", $"Can't add actuals to a non approved project. Partition with id {partition.Id}");
                }

                // If categories are to be changed, make sure that a category doesn't already exist with those categories
                if (partition.Categories == null) continue;
                var catHashset = partition.Categories.ToImmutableHashSet();
                foreach (var resourcePartition in resource.Partitions)
                {
                    if(resourcePartition.Id == partition.Id) continue;

                    var partitionCatHashset =
                        resourcePartition.Categories.Select(c => c.FinancialResourceCategory.Name).ToImmutableHashSet();
                    if (catHashset.SetEquals(partitionCatHashset))
                    {
                        result.AddError("Categories", $"Partion with id {resourcePartition.Id} already exists with this category set.");
                    }
                }
            }

            if (!result.Succeeded) return result;
            
            // Do the updates
            foreach (var partition in ps)
            {
                // If no current adjustments
                var p = _respository.FinancialResourcePartitions.First(frp => frp.Id == partition.Id);
                if (p.Adjustments.Count == 0)
                {
                    var start = new FinancialAdjustment()
                    {
                        Actual = partition.Actual,
                        Additive = false,
                        Date = p.FinancialResource.StartDate,
                        FinancialResourcePartition = p,
                        Value = partition.Adjustment
                    };
                    p.Adjustments.Add(start);
                    if (!p.FinancialResource.EndDate.HasValue) continue;
                    var end = new FinancialAdjustment()
                    {
                        FinancialResourcePartition = p,
                        Date = p.FinancialResource.EndDate.Value,
                        Actual = partition.Actual,
                        Additive = false,
                        Value = 0
                    };
                    p.Adjustments.Add(end);
                }
                else
                {
                    var adjustment = p.Adjustments.OrderBy(a => a.Date).First();
                    adjustment.Value = partition.Adjustment;
                    adjustment.Actual = partition.Actual;
                }

                // Update Categories
                if(partition.Categories == null) continue;

                var updatedCategoryHash = partition.Categories.ToImmutableHashSet();

                var categoryHash = p.Categories
                    .Select(c => c.FinancialResourceCategory.Name)
                    .ToImmutableHashSet();

                // Delete
                var catsToDelete = categoryHash
                    .Where(c => !updatedCategoryHash.Contains(c))
                    .Select(cat => p.Categories.Single(x => x.FinancialResourceCategory.Name == cat))
                    .Select(y => y.FinancialResourceCategory)
                    .ToList();

                await _respository.RemoveCategoriesFromFinancialPartitionAsync(p, catsToDelete);

                // Add
                var catsToAdd = updatedCategoryHash.Where(c => !categoryHash.Contains(c)).ToList();

                // Check for existing
                var addList = new List<FinancialResourceCategory>();
                foreach (var cat in catsToAdd)
                {
                    var newCat =
                        _respository.FinancialResourceCategories
                            .Where(frc => frc.GroupId == resource.ResourceScenario.Group.Id)
                            .SingleOrDefault(rc => rc.Name == cat);

                    if (newCat == null)
                    {
                        newCat = new FinancialResourceCategory()
                        {
                            Name = cat,
                            Group = resource.ResourceScenario.Group
                        };

                        await
                            AddFinancialResourceCategoriesAsync(resource.ResourceScenario.Group,
                                new List<FinancialResourceCategory> {newCat});
                    }

                    addList.Add(newCat);
                }

                await _respository.AddCategoriesToFinancialPartitionAsync(p, addList);
            }
            await _respository.SaveChangesAsync();
            return result;
        }

        public async Task<MerlinPlanBLResult> CopyFinancialResourcesAsync(IEnumerable<IResourceCopyRequest> request)
        {
            var result = new MerlinPlanBLResult();

            var copyRequests = request.ToArray();
            
            // Validate scenarios and copy targets exist
            foreach (var rcr in copyRequests)
            {
                if (_respository.ResourceScenarios.All(rs => rs.Id != rcr.ResourceScenario))
                {
                    result.AddError("ResourceScenario", $"Resource Scenario not found with id {rcr.ResourceScenario}");
                }

                if (_respository.FinancialResources.All(fr => fr.Id != rcr.Id))
                {
                    result.AddError("Id", $"Financial Resource not found with id {rcr.Id}");
                }
            }

            if (!result.Succeeded) return result;

            var resultData = new List<FinancialResource>();
            
            // Now process the copys
            foreach (var resourceCopyRequest in copyRequests)
            {
                var scenario = _respository.ResourceScenarios
                    .First(rs => rs.Id == resourceCopyRequest.ResourceScenario);

                var resource = _respository.FinancialResources
                    .First(fr => fr.Id == resourceCopyRequest.Id);

                // Duplicate resource
                var newResource = new FinancialResource
                {
                    StartDate = resource.StartDate,
                    EndDate = resource.EndDate,
                    Name = resourceCopyRequest.Name ?? $"{resource.Name} Copy",
                    ResourceScenario = scenario,
                    Partitions = new List<FinancialResourcePartition>(),
                    Recurring = resource.Recurring
                };

                await _respository.AddFinancialResourceAsync(newResource);


                // Duplicate resource partitions
                foreach (var partition in resource.Partitions.ToList())
                {
                    var newPartition = new FinancialResourcePartition()
                    {
                        Adjustments = new List<FinancialAdjustment>(),
                        FinancialResource = newResource,
                        Categories = new List<PartitionResourceCategory>()
                    };
                    await _respository.AddFinancialResourcePartitionAsync(newPartition);
                    var categories = partition.Categories.Select(prc => prc.FinancialResourceCategory).ToList();
                    await _respository.AddCategoriesToFinancialPartitionAsync(newPartition, categories);

                    // Add adjustments
                    foreach (var adjustment in partition.Adjustments.ToList())
                    {
                        var newAdjustment = new FinancialAdjustment()
                        {
                            FinancialResourcePartition = newPartition,
                            Date = adjustment.Date,
                            Actual = false,
                            Value = adjustment.Value,
                            Additive = adjustment.Additive,
                        };

                        await _respository.AddAdjustmentToFinancialResourceAsync(newAdjustment);
                    }
                }
                resultData.Add(newResource);
            }

            result.SetData(resultData);
            return result;
        }

        #endregion

        #region Resource Scenarios

        public async Task<MerlinPlanBLResult> CopyResourceScenariosAsync(IEnumerable<IDocumentCopyRequest> requests)
        {
            var result = new MerlinPlanBLResult();
            var copyRequests = requests.ToList();

            // Validation that the groups, users and scenarios actually exist
            foreach (var request in copyRequests)
            {
                if (_respository.ResourceScenarios.All(rs => rs.Id != request.Id))
                {
                    result.AddError("Id", $"The resource with id {request.Id} could not be found.");
                }

                if (_respository.Groups.All(g => g.Id != request.Group))
                {
                    result.AddError("Group", $"The group with id {request.Group} could not be found.");
                }

                if (_respository.Users.All(u => u.Id != request.User))
                {
                    result.AddError("User", $"The user with id {request.User} could not be found.");
                }    
            }
            
            if (!result.Succeeded) return result;

            var resultData = new List<ResourceScenario>();

            foreach (var request in copyRequests)
            {
                var scenario = _respository.ResourceScenarios.First(rs => rs.Id == request.Id);
                var group = _respository.Groups.First(g => g.Id == request.Group);
                var user = _respository.Users.First(u => u.Id == request.User);

                var newScenario = new ResourceScenario()
                {
                    Name = request.Name ?? $"{scenario.Name} Copy",
                    Approved = false,
                    ApprovedBy = null,
                    Creator = user,
                    Group = group,
                    FinancialResources = new List<FinancialResource>(),
                    StaffResources = new List<StaffResource>(),
                    ShareAll = false,
                    ShareGroup = false,
                    ShareUser = new List<ResourceScenarioUser>()
                };

                await _respository.AddResourceScenarioAsync(newScenario);

                // Add Financial and Staff resources
                var frcrs = scenario.FinancialResources.Select(fr => new CopyRequest
                {
                    ResourceScenario = newScenario.Id,
                    Name = fr.Name,
                    Id = fr.Id
                   
                }).ToList();

                await CopyFinancialResourcesAsync(frcrs);

                var srs = scenario.StaffResources.Select(sr => new CopyRequest
                {
                    ResourceScenario = newScenario.Id,
                    Name = sr.Name,
                    Id = sr.Id
                });

                await CopyStaffResourcesAsync(srs);

                resultData.Add(newScenario);
            }
            result.SetData(resultData);
            return result;
        }

        #endregion

        #region Staff Resources

        public async Task<MerlinPlanBLResult> UpdateStaffResourceAsync(StaffResource resource)
        {
            var result = new MerlinPlanBLResult();
            
            // Check that scenario isn't approved. If approved then we can't change
            if (resource.ResourceScenario.Approved)
            {
                result.AddError("Approved", "The resource belongs to an approved scenario and cannot be edited.");
            }

            if (!result.Succeeded) return result;

            if (resource.Adjustments.Count == 0)
            {
                var start = new StaffAdjustment()
                {
                    Date = resource.StartDate,
                    Actual = false,
                    Value = 0f,
                    Additive = false,
                    StaffResource = resource
                };
                resource.Adjustments.Add(start);
            }

            var startAdjustment = resource.Adjustments.OrderBy(a => a.Date).FirstOrDefault();
            if (startAdjustment.Date != resource.StartDate)
            {
                startAdjustment.Date = resource.StartDate;
            }

            if (resource.EndDate.HasValue)
            {
                if (resource.Adjustments.Count != 1)
                {
                    if (resource.Adjustments.Count == 2)
                    {
                        var end = resource.Adjustments.OrderByDescending(a => a.Date).First();
                        end.Date = resource.EndDate.Value;
                    }
                }
                else
                {
                    // Add a new end adjustment
                    var end = new StaffAdjustment
                    {
                        Date = resource.EndDate.Value,
                        Actual = false,
                        Value = 0f,
                        Additive = false,
                        StaffResource = resource
                    };
                    resource.Adjustments.Add(end);
                }
            }
            else
            {
                if (resource.Adjustments.Count == 2)
                {
                    var end = resource.Adjustments.OrderByDescending(a => a.Date).First();
                    resource.Adjustments.Remove(end);
                }
            }

            await _respository.SaveChangesAsync();
            return result;
        }

        public async Task<MerlinPlanBLResult> CopyStaffResourcesAsync(IEnumerable<IResourceCopyRequest> requests)
        {
            var result = new MerlinPlanBLResult();
            var copyRequests = requests.ToArray();
            var resultData = new List<StaffResource>();

            // Need to validate that resource scenario and copy source exist
            foreach (var copyRequest in copyRequests)
            {
                if (_respository.ResourceScenarios.All(rs => rs.Id != copyRequest.ResourceScenario))
                {
                    result.AddError("ResourceScenario", $"Resource Scenario not found with id {copyRequest.ResourceScenario}");
                }

                if (_respository.StaffResources.All(fr => fr.Id != copyRequest.Id))
                {
                    result.AddError("Id", $"Staff Resource not found with id {copyRequest.Id}");
                }
            }

            if (!result.Succeeded) return result;

            // Do updates
            foreach (var copyRequest in copyRequests)
            {
                var resource = _respository.StaffResources.Single(sr => sr.Id == copyRequest.Id);
                var scenario = _respository.ResourceScenarios.Single(rs => rs.Id == copyRequest.ResourceScenario);

                var newStaffResource = new StaffResource
                {
                    EndDate = resource.EndDate,
                    Adjustments = new List<StaffAdjustment>(),
                    Categories = new List<StaffResourceStaffResourceCategory>(),
                    Name = copyRequest.Name ?? $"{resource.Name} Copy",
                    ResourceScenario = scenario,
                    FteOutput = resource.FteOutput,
                    StartDate = resource.StartDate,
                    UserData = resource.UserData,
                    Recurring = resource.Recurring
                };

                await _respository.AddStaffResourceAsync(newStaffResource);

                // Copy Categories
                await _respository.AddCategoriesToStaffResourceAsync(
                    resource.Categories.Select(c => c.StaffResourceCategory).ToList(), 
                    newStaffResource);

                // Copy Adjustment
                foreach (var adjustment in resource.Adjustments)
                {
                    var newAdjustment = new StaffAdjustment()
                    {
                        Value = adjustment.Value,
                        StaffResource = newStaffResource,
                        Date = adjustment.Date,
                        Actual = adjustment.Actual,
                        Additive = adjustment.Additive,
                    };
                    newStaffResource.Adjustments.Add(newAdjustment);
                }
                
                await _respository.SaveChangesAsync();
                resultData.Add(newStaffResource);
            }

            result.SetData(resultData);
            return result;
        }

        #endregion

        #region Business Units

        public async Task<MerlinPlanBLResult> AddBusinessUnitAsync(BusinessUnit businessUnit)
        {
            var result = new MerlinPlanBLResult();
            
            // Check that name is unique for the org
            var validate =
                _respository.BusinessUnits.Where(bu => bu.OrganisationId == businessUnit.OrganisationId)
                    .All(bu => bu.Name != businessUnit.Name);

            if (validate)
            {
                await _respository.AddBusinessUnitAsync(businessUnit);
                return result;
            }

            result.AddError("Name", $"The business unit name {businessUnit.Name} is already used in this organisation.");
            return result;

        }

        public async Task<MerlinPlanBLResult> DeleteBusinessUnitAsync(BusinessUnit businessUnit)
        {
            var result = new MerlinPlanBLResult();
            // We need to make sure that the business unit is not used in any projects.
            if (businessUnit.ProjectsImpacting.Count > 0)
            {
                result.AddError("ProjectsImpacting", "There are projects that impact upon this business unit so it cannot be removed.");
            }

            if (businessUnit.ProjectsOwned.Count > 0)
            {
                result.AddError("ProjectsOwned", "There are projects that are owned by this business unit so it cannot be removed");
            }

            if (!result.Succeeded) return result;
            await _respository.RemoveBusinessUnitAsync(businessUnit);
            return result;
        }

        #endregion

        #region Project

        public async Task<MerlinPlanBLResult> DeleteProjectAsync(Project project)
        {
            var result = new MerlinPlanBLResult();
            // Need to make sure that the project isn't being used in any
            // portfolio document before we delete.
            if (_respository.Portfolios.Any(p => p.Projects.Exists(pr => pr.ProjectOption.ProjectId == project.Id)))
            {
                result.AddError("ProjectOption", "There are protfolios that use a project option from this project so it cannot be removed.");
            }

            if (!result.Succeeded) return result;
            await _respository.RemoveProjectAsync(project);
            return result;
        }

        public async Task<MerlinPlanBLResult> AddProjectAsync(Project project)
        {
            // check that required values are set


            var result = new MerlinPlanBLResult();
            await _respository.AddProjectAsync(project);
            return result;
        }

        public async Task<MerlinPlanBLResult> AddProjectPhaseAsync(ProjectPhase phase)
        {
            await _respository.AddProjectPhaseAsync(phase);
            return new MerlinPlanBLResult();
        }

        public async Task<MerlinPlanBLResult> DeleteProjectPhaseAsync(ProjectPhase phase)
        {
            await _respository.RemoveProjectPhaseAsync(phase);
            return new MerlinPlanBLResult();
        }

        public async Task<MerlinPlanBLResult> CopyProjectAsync(IEnumerable<IDocumentCopyRequest> requests)
        {
            var result = new MerlinPlanBLResult();

            var copyRequests = requests.ToList();
            
            // Additional Model validation
            foreach (var request in copyRequests)
            {
                if (_respository.Projects.All(p => p.Id != request.Id))
                {
                    result.AddError("Id", $"The resource with id {request.Id} could not be found.");
                }

                if (_respository.Groups.All(g => g.Id != request.Group))
                {
                    result.AddError("Group", $"The group with id {request.Group} could not be found.");
                }

                if (_respository.Users.All(u => u.Id != request.User))
                {
                    result.AddError("User", $"The user with id {request.User} could not be found.");
                }
            }

            if (!result.Succeeded) return result;

            var resultData = new List<Project>();

            foreach (var request in copyRequests)
            {
                var project = _respository.Projects.Single(p => p.Id == request.Id);

                // Do the copy
                var newProject = new Project
                {
                    Name = request.Name ?? $"{project.Name} Copy",
                    Creator = _respository.Users.Single(u => u.Id == request.User),
                    Group = _respository.Groups.Single(g => g.Id == request.Group),
                    ImpactedBusinessUnit = project.ImpactedBusinessUnit,
                    OwningBusinessUnit = project.OwningBusinessUnit,
                    Reference = project.Reference,
                    Summary = project.Summary,
                };

                await _respository.AddProjectAsync(newProject);

                // Add related project data
                var frcs = project.FinancialResourceCategories
                    .Select(frc => new ProjectFinancialResourceCategory
                    {
                        FinancialResourceCategory = frc.FinancialResourceCategory,
                        Project = newProject
                    }).ToList();

                newProject.FinancialResourceCategories = new List<ProjectFinancialResourceCategory>(frcs);

                var projectOptions = _respository.ProjectOptions.Where(po => po.ProjectId == project.Id).ToList();

                // Clone options
                foreach (var option in projectOptions)
                {
                    var newOption = new ProjectOption
                    {
                        Complexity = option.Complexity,
                        Description = option.Description,
                        Priority = option.Priority,
                        Project = newProject
                    };

                    await _respository.AddProjectOptionAsync(newOption);

                    // Add phases
                    foreach (var phase in option.Phases)
                    {
                        var newPhase = new ProjectPhase
                        {
                            Description = phase.Description,
                            EndDate = phase.EndDate,
                            DesiredEndDate = phase.DesiredEndDate,
                            DesiredStartDate = phase.DesiredStartDate,
                            Name = phase.Name,
                            ProjectOption = newOption,
                            StartDate = phase.StartDate
                        };

                        await _respository.AddProjectPhaseAsync(newPhase);

                        newPhase.FinancialResources = new List<FinancialTransaction>();

                        // Add financial transactions
                        foreach (var ft in phase.FinancialResources)
                        {

                            var newFinancialTransation = new FinancialTransaction
                            {
                                Actual = ft.Actual,
                                Additive = ft.Actual,
                                Date = ft.Date,
                                ProjectPhase = newPhase,
                                Reference = ft.Reference,
                                Value = ft.Value
                            };

                            var ftrcs = ft.Categories.Select(f => new FinancialTransactionResourceCategory
                            {
                                FinancialResourceCategory = f.FinancialResourceCategory,
                                FinancialTransaction = newFinancialTransation
                            });

                            newFinancialTransation.Categories = new List<FinancialTransactionResourceCategory>(ftrcs);
                            newPhase.FinancialResources.Add(newFinancialTransation);
                        }

                        newPhase.StaffResources = new List<StaffTransaction>();
                        foreach (var staffResource in phase.StaffResources)
                        {
                            var newStaffTransaction = new StaffTransaction
                            {
                                Actual = staffResource.Actual,
                                Additive = staffResource.Additive,
                                Date = staffResource.Date,
                                ProjectPhase = newPhase,
                                StaffResource = staffResource.StaffResource,
                                Category = staffResource.Category,
                                Value = staffResource.Value
                            };

                            newPhase.StaffResources.Add(newStaffTransaction);
                        }

                        await _respository.SaveChangesAsync();
                    }

                    var deps = option.RequiredBy.ToArray();

                    foreach (var dependency in deps)
                    {
                        await _respository.AddProjectDependencyAsync(dependency.RequiredBy, newOption);
                    }

                    foreach (var benefit in option.Benefits)
                    {
                        var newBenefit = new ProjectBenefit
                        {
                            Achieved = benefit.Achieved,
                            AchievedValue = benefit.AchievedValue,
                            Date = benefit.Date,
                            Description = benefit.Description,
                            Name = benefit.Name,
                            ProjectOption = newOption
                        };

                        newBenefit.Alignments = benefit.Alignments
                            .Select(a => new Alignment
                            {
                                Actual = a.Actual,
                                AlignmentCategory = a.AlignmentCategory,
                                Date = a.Date,
                                ProjectBenefit = newBenefit,
                                Value = a.Value,
                                AlignmentCategoryId = a.AlignmentCategoryId,
                                ProjectBenefitId = newBenefit.Id

                            })
                            .ToList();

                        newBenefit.Categories = benefit.Categories.Select(bc => new ProjectBenefitBenefitCategory
                        {
                            BenefitCategory = bc.BenefitCategory,
                            ProjectBenefit = newBenefit,
                            BenefitCategoryId = bc.BenefitCategoryId
                        }).ToList();

                        await AddProjectBenefitAsync(newBenefit);
                    }

                    newOption.RiskProfile = option.RiskProfile.Select(rp => new RiskProfile
                    {
                        Actual = rp.Actual,
                        Date = rp.Date,
                        Impact = rp.Impact,
                        Mitigation = rp.Mitigation,
                        Probability = rp.Probability,
                        ProjectOption = newOption,
                        Residual = rp.Residual,
                        RiskCategoryId = rp.RiskCategoryId
                    }).ToList();
                }
                await _respository.SaveChangesAsync();
                resultData.Add(newProject);
            }
            result.SetData(resultData);
            return result;
        }

        public async Task<MerlinPlanBLResult> UpdateProjectAsync(IEnumerable<IProjectUpdate> requests)
        {
            var result = new MerlinPlanBLResult();

            var updateRequests = requests.ToList();

            // validate requests
            foreach (var request in updateRequests)
            {
                // Check for existance
                if (_respository.Projects.All(p => p.Id != request.Id))
                {
                    result.AddError("Id", $"A project with id: {request.Id} can't be found.");
                }

                // Check financial resource category existance
                if(request.Categories == null) continue;

                foreach (var category in request.Categories)
                {
                    var project = _respository.Projects.SingleOrDefault(p => p.Id == request.Id);
                    if(project == null) continue;
                    if (_respository.FinancialResourceCategories
                            .Where(frc => frc.GroupId == project.Group.Id)
                            .All(frc => frc.Name != category)
                            )
                    {
                        result.AddError("Categories", $"There is no category in this project's group with the name {category}");
                    }
                }

                var proj = _respository.Projects.SingleOrDefault(pr => pr.Id == request.Id);

                if(proj == null) continue;

                // Validate owning and impacted business units
                if (_respository.BusinessUnits.Where(bu => bu.OrganisationId == proj.Group.OrganisationId).All(bu => bu.Name != request.ImpactedBusinessUnit))
                {
                    result.AddError("ImpactedBusinessUnit", $"The business unit {request.ImpactedBusinessUnit} could not be found");
                }

                if (_respository.BusinessUnits.Where(bu => bu.OrganisationId == proj.Group.OrganisationId).All(bu => bu.Name != request.OwningBusinessUnit))
                {
                    result.AddError("OwningBusinessUnit", $"The business unit {request.OwningBusinessUnit} could not be found");
                }
            }

            if (!result.Succeeded) return result;

            // update projects
            foreach (var request in updateRequests)
            {
                var project = _respository.Projects.Single(p => p.Id == request.Id);
                project.Name = request.Name;
                project.Summary = request.Summary;
                project.Reference = request.Reference;
                project.ImpactedBusinessUnit =
                    _respository.BusinessUnits.Single(bu => bu.Name == request.ImpactedBusinessUnit);
                project.OwningBusinessUnit =
                    _respository.BusinessUnits.Single(bu => bu.Name == request.OwningBusinessUnit);

                
                if(request.Categories == null) continue;
                
                // Update categories
                var updatedCategoryIds = request.Categories
                        .Select(c => _respository.FinancialResourceCategories
                            .Where(frc => frc.GroupId == project.Group.Id)
                            .Single(frc => frc.Name == c))
                        .Select(frc => frc.Id)
                        .ToImmutableHashSet();

                var currentCategoryIds =
                    project.FinancialResourceCategories
                    .Select(frc => frc.FinancialResourceCategoryId)
                    .ToImmutableHashSet();

                var catsToDelete =
                    project.FinancialResourceCategories.Where(
                        frc => !updatedCategoryIds.Contains(frc.FinancialResourceCategoryId));

                var catIdsToAdd = updatedCategoryIds.Where(cId => !currentCategoryIds.Contains(cId));

                // Delete
                foreach (var category in catsToDelete)
                {
                    project.FinancialResourceCategories.Remove(category);
                }

                // Add
                await
                    _respository.AddFinancialResourceCategoriesToProjectAsync(project,
                        catIdsToAdd.Select(cId => _respository.FinancialResourceCategories.Single(frc => frc.Id == cId)));

            }
            await _respository.SaveChangesAsync();
            result.SetData(updateRequests.Select(r => _respository.Projects.Single(p => p.Id == r.Id)).ToList());
            return result;
        }

        public async Task<MerlinPlanBLResult> DeleteProjectOptionAsync(ProjectOption option)
        {
            var result = new MerlinPlanBLResult();

            // Need to disallow delete if option is used in a current portfolio which is approved.
            var portfolios =
                _respository.Portfolios.Where(p => p.Approved)
                    .Where(p => p.Projects.Any(pr => pr.ProjectOptionId == option.Id))
                    .ToList();


            foreach (var portfolio in portfolios)
            {
                result.AddError("Portfolio", $"approved portfolio with id: {portfolio.Id} is currently using this option so it cannot be deleted.");
            }
            
            // Need to disallow delete if there are any dependencies on this project
            var deps =
                _respository.ProjectOptions.Where(po => po.RequiredBy.Any(d => d.DependsOnId == option.Id)).ToList();

            foreach (var dep in deps)
            {
                result.AddError("Dependencies", $"the project option with id: {dep.Id} depends on this option so it cannot be deleted.");
            }    
             
            if (!result.Succeeded) return result;
            await _respository.RemoveProjectOptionAsync(option);
            return result;
        }

        public async Task<MerlinPlanBLResult> AddBenefitCategoriesAsync(Group group, IEnumerable<BenefitCategory> categories)
        {
            var categoryList = categories.ToList();
            var result = new MerlinPlanBLResult();
            foreach (var category in categoryList)
            {
                if (group.BenefitCategories.Any(frc => frc.Name == category.Name))
                {
                    result.AddError("Name", $"The Benefit Category Name {category.Name} is already used in this group");
                }
            }

            if (!result.Succeeded) return result;
            {
                foreach (var category in categoryList)
                {
                    await _respository.AddBenefitCategoryAsync(category);
                }
            }
            return result;
        }

        public async Task<MerlinPlanBLResult> DeleteBenefitCategoryAsync(BenefitCategory category)
        {
            var result = new MerlinPlanBLResult();

             // Disalow if there are any project options using the category
            if (category.ProjectBenefits.Count > 0)
            {
                var optionIds = category.ProjectBenefits.Select(pbbc => pbbc.ProjectBenefit.ProjectOptionId);
                foreach (var optionId in optionIds)
                {
                    result.AddError("ProjectBenefit", $"Cannot delete benefit category because project option with id {optionId} has a benefit using it.");
                }
            }

            if (!result.Succeeded) return result;

            await _respository.RemoveBenefitCategoryAsync(category);
            return result;
        }

        public async Task<MerlinPlanBLResult> AddProjectBenefitAsync(ProjectBenefit benefit)
        {
            await _respository.AddProjectBenefitAsync(benefit);
            return new MerlinPlanBLResult();
        }

        public async Task<MerlinPlanBLResult> DeleteProjectBenefitAsync(ProjectBenefit benefit)
        {
            await _respository.RemoveProjectBenefitAsync(benefit);
            return new MerlinPlanBLResult();
        }

        public async Task<MerlinPlanBLResult> DeleteAlignmentCategoryAsync(AlignmentCategory category)
        {
            var result = new MerlinPlanBLResult();

            foreach (var a in category.Alignments)
            {
                result.AddError("Alignment", $"Cannot delete alignment category because project option with id {a.ProjectBenefit.ProjectOptionId} has a benefit with id {a.ProjectBenefitId} using it.");
            }
            if (!result.Succeeded) return result;
            await _respository.RemoveAlignmentCategoryAsync(category);
            return result;
        }

        public async Task<MerlinPlanBLResult> DeleteRiskCategoryAsync(RiskCategory category)
        {
            var result = new MerlinPlanBLResult();
            foreach (var riskProfile in category.RiskProfiles)
            {
                result.AddError("RiskProfile", $"Cannot delete risk category because project option with id {riskProfile.ProjectOptionId} has a riskprofile with id {riskProfile.Id} using it.");
            }
            if (!result.Succeeded) return result;
            await _respository.RemoveRiskCategoryAsync(category);
            return result;
        }

        #endregion

        #region Portfolios

        public async Task<MerlinPlanBLResult> DeletePortfolioAsync(Portfolio portfolio)
        {
            var result = new MerlinPlanBLResult();
            if(portfolio.Approved) result.AddError("Approved", "Approved portfolios cannot be deleted.");
            if (!result.Succeeded) return result;
            await _respository.RemovePortfolioAsync(portfolio);
            return result;
        }

        public async Task<MerlinPlanBLResult> AddPortfolioAsync(Portfolio portfolio)
        {
            await _respository.AddPortfolioAsync(portfolio);
            return new MerlinPlanBLResult();
        }

        public async Task<MerlinPlanBLResult> UpdatePortfolioAsync(IEnumerable<IPortfolioUpdate> requests)
        {
            var result = new MerlinPlanBLResult();

            var portfolioUpdates = requests.ToList();

            foreach (var portfolio in portfolioUpdates)
            {
                var p = _respository.Portfolios.SingleOrDefault(pf => pf.Id == portfolio.Id);
                if (p == null)
                {
                    result.AddError("Id", $"A portfolio matching id {portfolio.Id} was not found.");
                    continue;
                }

                if(p.Approved) result.AddError("Approved", $"Portfolio with id {portfolio.Id} is approved and cannot be modified.");
            }

            if (!result.Succeeded) return result;

            var resultData = new List<Portfolio>();

            foreach (var update in portfolioUpdates)
            {
                var portfolio = _respository.Portfolios.Single(p => p.Id == update.Id);
                portfolio.Name = update.Name ?? portfolio.Name;
                portfolio.StartYear = update.StartYear ?? portfolio.StartYear;
                portfolio.EndYear = update.EndYear ?? portfolio.EndYear;
                portfolio.TimeScale = update.TimeScale ?? portfolio.TimeScale;
                resultData.Add(portfolio);
            }

            await _respository.SaveChangesAsync();
            result.SetData(resultData);
            return result;
        }


        public async Task<MerlinPlanBLResult> AddProjectToPortfolioAsync(Portfolio portfolio, IEnumerable<IAddProjectToPortfolioRequest> requests)
        {
            var result = new MerlinPlanBLResult();

            var projects = requests.ToArray();

            // Validation
            foreach (var request in projects)
            {
                // Check that project exists
                if (_respository.Projects.All(p => p.Id != request.ProjectId))
                {
                    result.AddError("ProjectId", $"A project with an id of {request.ProjectId} cannot be found.");
                }

                // Check that owner exists
                if (
                    request.Owner != null &&
                    _respository.StaffResources
                        .All(sr => sr.Id != request.Owner)
                    )
                {
                    result.AddError("Owner", $"A staff resource with an id of {request.Owner} cannot be found");
                }

                // Check that managers exist
                if (request.Managers != null)
                {
                    foreach (var manager in request.Managers)
                    {
                        if (
                            _respository.StaffResources
                                .All(sr => sr.Id != manager))
                        {
                            result.AddError("Managers", $"The manager with staff resource id of {manager} could not be found.");
                        }
                    }
                }

                // Check that options exist and that option is part of specified project.
                if (
                    _respository.ProjectOptions
                        .Where(po => po.ProjectId == request.ProjectId)
                        .All(po => po.Id != request.OptionId))
                {
                    result.AddError("OptionId", $"The project option with id {request.OptionId} cannot be found or is not an option in project with id {request.ProjectId}.");
                }
            }

            if (!result.Succeeded) return result;

            var resultData = new List<ProjectConfig>();

            foreach (var request in projects)
            {
                var projectOption = _respository.ProjectOptions.Single(po => po.Id == request.OptionId);
                var firstPhase = projectOption.Phases.OrderBy(pp => pp.StartDate).First();

                // Use the phases actual start date over desired if it exists
                var firstPhaseStartDate = firstPhase.StartDate ?? firstPhase.DesiredStartDate;

                // The difference between the estimated start date if it exists
                var phaseOffset = request.EstimatedStartDate - firstPhaseStartDate;

                // If startdate is null then use date of first phase
                var projectConfigStart = request.EstimatedStartDate ?? firstPhaseStartDate;

                // Copy properties
                var newProjectConfig = new ProjectConfig
                {
                    PortfolioId = portfolio.Id,
                    StartDate = projectConfigStart,
                    OwnerId = request.Owner,
                    ProjectOptionId = request.OptionId
                };

                await _respository.AddProjectConfigAsync(newProjectConfig);

                // Add managers
                newProjectConfig.Managers = request.Managers.Select(m => new StaffResourceProjectConfig
                {
                    ProjectConfigId = newProjectConfig.Id,
                    StaffResourceId = m
                }).ToList();

                foreach (var tag in request.Tags)
                {
                    if (portfolio.PortfolioTags.All(pt => pt.Name != tag))
                    {
                        await _respository.AddTagToPortfolioAsync(portfolio, tag);
                    }
                }

                await _respository.AddTagsToProjectConfigAsync(newProjectConfig, request.Tags);

                // Create phase configs
                // Create a default set of phase configs based on the project phases
                // Work out phase offset

                newProjectConfig.Phases = new List<PhaseConfig>();
                foreach (var phase in projectOption.Phases)
                {
                    var newPhaseConfig = new PhaseConfig
                    {
                        StartDate = phase.StartDate ?? phase.DesiredStartDate,
                        EndDate = phase.EndDate ?? phase.DesiredEndDate,
                        ProjectConfigId = newProjectConfig.Id,
                        ProjectPhaseId = phase.Id
                    };

                    if (phaseOffset != null)
                    {
                        newPhaseConfig.StartDate += phaseOffset.Value;
                        newPhaseConfig.EndDate += phaseOffset.Value;
                    }

                    newProjectConfig.Phases.Add(newPhaseConfig);
                }
                await _respository.SaveChangesAsync();
                resultData.Add(newProjectConfig);
            }
            result.SetData(resultData);
            return result;
        }
        #endregion
    }
}
