using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System;

namespace MPWebAPI.Models
{
    public class MerlinPlanRepository : IMerlinPlanRepository
    {
        private readonly DBContext _dbcontext;
        private readonly UserManager<MerlinPlanUser> _userManager;
        private readonly ILogger _logger;
        
        public MerlinPlanRepository(
            DBContext dbcontext,
            UserManager<MerlinPlanUser> userManager,
            ILoggerFactory loggerFactory
            )
        {
            _dbcontext = dbcontext;
            _logger = loggerFactory.CreateLogger("MerlinPlanRepository");
            _userManager = userManager;
        }

        public async Task SaveChangesAsync()
        {
            await _dbcontext.SaveChangesAsync();
        }

        #region Group

        public IEnumerable<Group> Groups
        {
            get
            {
                return _dbcontext.Group
                    .Include(g => g.ResourceScenarios)
                    .Include(g => g.FinancialResourceCategories)
                    .ToList();
            }
        }

        public async Task AddGroupAsync(Group g)
        {
            _dbcontext.Group.Add(g);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task RemoveGroupAsync(Group g)
        {
            _dbcontext.Group.Remove(g);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<IEnumerable<MerlinPlanUser>> GetGroupMembersAsync(Group g)
        {
            return await _dbcontext.UserGroup
                .Where(ug => ug.GroupId == g.Id)
                .Select(ug => ug.User).ToListAsync();
        }

        public async Task AddUserToGroupAsync(MerlinPlanUser user, Group group)
        {
            // Check to see that user is not already in the group
            var exists = await _dbcontext.UserGroup
                .AnyAsync(ug => ug.GroupId == @group.Id && ug.UserId == user.Id);

            if (!exists)
            {
                var userGroup = new UserGroup();
                userGroup.Group = @group;
                userGroup.User = user;
                _dbcontext.UserGroup.Add(userGroup);
                await _dbcontext.SaveChangesAsync();
            }
        }

        public async Task RemoveUserFromGroupAsync(MerlinPlanUser user, Group group)
        {
            var exists = await _dbcontext.UserGroup
                .Where(ug => ug.GroupId == @group.Id && ug.UserId == user.Id)
                .FirstOrDefaultAsync();
            
            if (exists != null)
            {
                _dbcontext.UserGroup.Remove(exists);
                await _dbcontext.SaveChangesAsync();
            }
        }

        public async Task ParentGroupAsync(Group child, Group parent)
        {
            child.Parent = parent;
            var result = await _dbcontext.SaveChangesAsync();
        }

        public async Task UnparentGroupAsync(Group group)
        {
            @group.Parent = null;
            await _dbcontext.SaveChangesAsync();
        }

        public async Task GroupSetActive(Group g, bool active)
        {
            g.Active = active;
            await _dbcontext.SaveChangesAsync();
        }

        #endregion

        #region Organisation

        public IEnumerable<Organisation> Organisations
        {
            get
            {
                return _dbcontext.Organisation;
            }
        }

        public async Task AddOrganisationAsync(Organisation org)
        {
            _dbcontext.Organisation.Add(org);
            await _dbcontext.SaveChangesAsync();
        }

        public IEnumerable<Group> GetOrganisationGroups(Organisation org)
        {
            return _dbcontext.Group.Where(g => g.OrganisationId == org.Id);
        }

        public async Task RemoveOrganisationAsync(Organisation org)
        {
            _dbcontext.Organisation.Remove(org);
            await _dbcontext.SaveChangesAsync();
        }

        #endregion

        #region User

        public IEnumerable<MerlinPlanUser> Users
        {
            get
            {
                return _userManager.Users
                    .Include(u => u.Organisation)
                    .ToList();
            }
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(MerlinPlanUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> UpdateUserAsync(MerlinPlanUser user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> RemoveUserFromRolesAsync(MerlinPlanUser user, IEnumerable<string> rolesToDelete)
        {
            return await _userManager.RemoveFromRolesAsync(user, rolesToDelete);
        }

        public async Task<IdentityResult> AddUserToRolesAsync(MerlinPlanUser user, IEnumerable<string> rolesToAdd)
        {
            return await _userManager.AddToRolesAsync(user, rolesToAdd);
        }

        public async Task<IEnumerable<Group>> GetUserGroupsAsync(MerlinPlanUser user)
        {
            return await _dbcontext.UserGroup
                .Where(ug => ug.UserId == user.Id)
                .Select(ug => ug.Group)
                .ToListAsync();
        }

        public async Task<IdentityResult> CreateUserAsync(MerlinPlanUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<MerlinPlanUser> FindUserByUserNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        #endregion

        #region Resource Scenario

        public IEnumerable<ResourceScenario> ResourceScenarios
        {
            get
            {
                return _dbcontext.ResourceScenario
                    .Include(rs => rs.Creator)
                    .Include(rs => rs.ApprovedBy)
                    .Include(rs => rs.Group);
            }
        }

        public async Task<IEnumerable<ResourceScenario>> GetUserSharedResourceScenariosForUserAsync(MerlinPlanUser user)
        {
            return await _dbcontext.ResourceScenarioUser
                .Where(rsu => rsu.UserId == user.Id)
                .Select(rsu => rsu.ResourceScenario).ToListAsync();
        }

        public async Task<IEnumerable<ResourceScenario>> GetGroupSharedResourceScenariosForUserAsync(MerlinPlanUser user)
        {
            var groups = await _dbcontext.UserGroup
                .Include(ug => ug.Group)
                .ThenInclude(g => g.ResourceScenarios)
                .Where(ug => ug.UserId == user.Id).ToListAsync();
            
            return groups
                .Where(ug => ug.Group.ResourceScenarios != null)
                .Select(ug => ug.Group).SelectMany(g => g.ResourceScenarios)
                .Where(rs => rs.ShareGroup);
        }

        public async Task<IEnumerable<ResourceScenario>> GetOrganisationSharedResourceScenariosAsync(Organisation org)
        {
            return await _dbcontext.ResourceScenario
                .Include(rs => rs.Group)
                .Where(rs => rs.Group.OrganisationId == org.Id && rs.ShareAll)
                .ToListAsync();
        }

        public async Task ShareResourceScenarioWithGroupAsync(ResourceScenario scenario, bool share)
        {
            scenario.ShareGroup = share;
            await _dbcontext.SaveChangesAsync();
        }

       
        public async Task ShareResourceScenarioWithOrgAsync(ResourceScenario scenario, bool share)
        {
            scenario.ShareAll = share;
            await _dbcontext.SaveChangesAsync();
        }

        public async Task ShareResourceScenarioWithUserAsync(ResourceScenario scenario, MerlinPlanUser user)
        {
            
            var r = await _dbcontext.ResourceScenarioUser
                .FirstOrDefaultAsync(rsu => rsu.ResourceScenarioId == scenario.Id && rsu.UserId == user.Id);
            
            if (r == null)
            {
                var rsu = new ResourceScenarioUser {
                    UserId = user.Id,
                    ResourceScenarioId = scenario.Id
                };
                _dbcontext.Add(rsu);
                await _dbcontext.SaveChangesAsync();
            }
        }

        public async Task UnshareResourceScenarioWithUserAsync(ResourceScenario scenario, MerlinPlanUser user)
        {
            var r = await _dbcontext.ResourceScenarioUser
                .FirstOrDefaultAsync(rsu => rsu.ResourceScenarioId == scenario.Id && rsu.UserId == user.Id);
            
            if (r != null)
            {
                _dbcontext.ResourceScenarioUser.Remove(r);
                await _dbcontext.SaveChangesAsync();
            }
        }

        public async Task RemoveResourceScenarioAsync(ResourceScenario scenario)
        {
            _dbcontext.ResourceScenario.Remove(scenario);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task AddResourceScenarioAsync(ResourceScenario scenario)
        {
            _dbcontext.ResourceScenario.Add(scenario);
            await _dbcontext.SaveChangesAsync();
        }

        #endregion

        #region Financial Resource

        public IEnumerable<FinancialResource> FinancialResources
        {
            get 
            {
                return _dbcontext.FinancialResource
                    .Include(fr => fr.Partitions).ThenInclude(frp => frp.Categories).ThenInclude(c => c.FinancialResourceCategory)
                    .Include(fr => fr.Partitions).ThenInclude(frp => frp.Adjustments)
                    .Include(fr => fr.ResourceScenario).ThenInclude(rs => rs.Group)
                    .ToList();
            }
        }

        public IEnumerable<FinancialResourceCategory> FinancialResourceCategories
        {
            get
            {
                return _dbcontext.FinancialResourceCategory
                    .Include(frc => frc.Group)
                    .Include(frc => frc.FinancialPartitions)
                    .Include(frc => frc.Transactions)
                    .ToList();
            }
        }

        public IEnumerable<FinancialResourcePartition> FinancialResourcePartitions
        {
            get
            {
                return _dbcontext.FinancialResourcePartition
                    .Include(frp => frp.Adjustments)
                    .Include(frp => frp.FinancialResource)
                    .Include(frp => frp.Categories)
                    .ThenInclude(prc => prc.FinancialResourceCategory)
                    .ToList();
            }
        }

        public async Task AddFinancialResourceAsync(FinancialResource resource)
        {
            _dbcontext.FinancialResource.Add(resource);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task RemoveFinancialResourceAsync(FinancialResource resource)
        {
            _dbcontext.FinancialResource.Remove(resource);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task AddFinancialResourceCategoryAsync(FinancialResourceCategory category)
        {
            _dbcontext.FinancialResourceCategory.Add(category);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task RemoveFinancialResourceCategoryAsync(FinancialResourceCategory category)
        {
            _dbcontext.Remove(category);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task AddFinancialResourcePartitionAsync(FinancialResourcePartition partition)
        {
            _dbcontext.FinancialResourcePartition.Add(partition);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task RemoveFinancialResourcePartitionAsync(FinancialResourcePartition partition)
        {
            _dbcontext.FinancialResourcePartition.Remove(partition);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task AddCategoriesToFinancialPartitionAsync(FinancialResourcePartition partition,
            IEnumerable<FinancialResourceCategory> categories)
        {
            var partitionCategories =
                categories.Select(
                    c =>
                        new PartitionResourceCategory
                        {
                            FinancialResourceCategory = c,
                            FinancialResourcePartition = partition
                        }).ToList();

            _dbcontext.PartitionResourceCategory.AddRange(partitionCategories);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task RemoveCategoriesFromFinancialPartitionAsync(FinancialResourcePartition partition,
            IEnumerable<FinancialResourceCategory> categories)
        {
            var prcs = _dbcontext.PartitionResourceCategory
                .Where(p => p.FinancialResourcePartitionId == partition.Id && categories.Contains(p.FinancialResourceCategory));
            
            _dbcontext.PartitionResourceCategory.RemoveRange(prcs);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task AddAdjustmentToFinancialResourceAsync(FinancialAdjustment adjustment)
        {
            _dbcontext.FinancialAdjustment.Add(adjustment);
            await _dbcontext.SaveChangesAsync();
        }

        #endregion

        #region Staff Resource

        public IEnumerable<StaffResource> StaffResources
        {
            get
            {
                return _dbcontext.StaffResource
                    .Include(sr => sr.Categories)
                    .ThenInclude(src => src.StaffResourceCategory)
                    .ToList();
            }
        }

        public async Task AddStaffResourceAsync(StaffResource resource)
        {
            _dbcontext.StaffResource.Add(resource);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task RemoveStaffResourceAsync(StaffResource resource)
        {
            _dbcontext.StaffResource.Remove(resource);
            await _dbcontext.SaveChangesAsync();
        }

        #endregion
    }    
}