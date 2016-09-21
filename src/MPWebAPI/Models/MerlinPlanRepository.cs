using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MPWebAPI.Models
{
    public class MerlinPlanRepository : IMerlinPlanRepository
    {
        private readonly PostgresDBContext _dbcontext;
        
        public MerlinPlanRepository(
            PostgresDBContext dbcontext
            )
        {
            _dbcontext = dbcontext;
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
                return _dbcontext.Group;
            }
        }

        public Task AddGroup(Group g)
        {
            throw new NotImplementedException();
        }

        public async Task AddOrganisation(Organisation org)
        {
            _dbcontext.Organisation.Add(org);
            await _dbcontext.SaveChangesAsync();
        }

        public Task RemoveGroup(Group g)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveOrganisation(Organisation org)
        {
            _dbcontext.Organisation.Remove(org);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task SaveChanges()
        {
            await _dbcontext.SaveChangesAsync();
        }
    }    
}