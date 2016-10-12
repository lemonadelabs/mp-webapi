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
        private readonly PostgresDBContext _dbcontext;
        private readonly UserManager<MerlinPlanUser> _userManager;
        private readonly ILogger _logger;
        
        public MerlinPlanRepository(
            PostgresDBContext dbcontext,
            UserManager<MerlinPlanUser> userManager,
            ILoggerFactory loggerFactory
            )
        {
            _dbcontext = dbcontext;
            _logger = loggerFactory.CreateLogger("MerlinPlanRepository");
            _userManager = userManager;
        }
       
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

        public IEnumerable<Organisation> Organisations
        {
            get
            {
                return _dbcontext.Organisation;
            }
        }

        public IEnumerable<Group> GetOrganisationGroups(Organisation org)
        {
            return _dbcontext.Group.Where(g => g.OrganisationId == org.Id);
        }

        public IEnumerable<Group> Groups
        {
            get
            {
                return _dbcontext.Group
                    .Include(g => g.ResourceScenarios);
            }
        }

        public IEnumerable<MerlinPlanUser> Users
        {
            get
            {
                return _userManager.Users
                    .Include(u => u.Organisation)
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

        public async Task AddOrganisationAsync(Organisation org)
        {
            _dbcontext.Organisation.Add(org);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task RemoveOrganisationAsync(Organisation org)
        {
            _dbcontext.Organisation.Remove(org);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
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
                .AnyAsync(ug => ug.GroupId == group.Id && ug.UserId == user.Id);

            if (!exists)
            {
                var userGroup = new UserGroup();
                userGroup.Group = group;
                userGroup.User = user;
                _dbcontext.UserGroup.Add(userGroup);
                await _dbcontext.SaveChangesAsync();
            }
        }

        public async Task RemoveUserFromGroupAsync(MerlinPlanUser user, Group group)
        {
            var exists = await _dbcontext.UserGroup
                .Where(ug => ug.GroupId == group.Id && ug.UserId == user.Id)
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
            group.Parent = null;
            await _dbcontext.SaveChangesAsync();
        }

        public async Task GroupSetActive(Group g, bool active)
        {
            g.Active = active;
            await _dbcontext.SaveChangesAsync();
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
                .Include(ug => ug.Group)
                .ThenInclude(g => g.ResourceScenarios)
                .Where(ug => ug.UserId == user.Id)
                .Select(ug => ug.Group)
                .ToListAsync();
        }

        public async Task<IdentityResult> CreateUserAsync(MerlinPlanUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IEnumerable<ResourceScenario>> GetUserSharedResourceScenariosForUserAsync(MerlinPlanUser user)
        {
            var populatedUser = await _userManager.Users
                .Include(u => u.SharedResourceScenarios)
                .ThenInclude(rsu => rsu.ResourceScenario)
                .Where(u => u.Id == user.Id)
                .FirstAsync();
            
            return populatedUser.SharedResourceScenarios?.Select(srs => srs.ResourceScenario);
        }

        public async Task<IEnumerable<ResourceScenario>> GetGroupSharedResourceScenariosForUserAsync(MerlinPlanUser user)
        {
            var groups = await GetUserGroupsAsync(user);
            return groups
                .Where(gr => gr.ResourceScenarios != null)
                .SelectMany(g => g.ResourceScenarios)
                .Where(rs => rs.ShareGroup)
                .ToList();
        }

        public async Task<IEnumerable<ResourceScenario>> GetOrganisationSharedResourceScenariosAsync(Organisation org)
        {
            return await _dbcontext.ResourceScenario
                .Include(rs => rs.Group)
                .Where(rs => rs.Group.OrganisationId == org.Id && rs.ShareAll)
                .ToListAsync();
        }
    }    
}