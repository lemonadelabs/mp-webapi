using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MPWebAPI.Models
{
    public class MerlinPlanRepository : IMerlinPlanRepository
    {
        private readonly PostgresDBContext _dbcontext;
        
        public MerlinPlanRepository(PostgresDBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public IEnumerable<Organisation> Organisations
        {
            get
            {
                return _dbcontext.Organisation
                    .Include(o => o.Groups)
                    .Include(o => o.Users);
            }
        }

        public async Task AddOrganisation(Organisation org)
        {
            _dbcontext.Organisation.Add(org);
            await _dbcontext.SaveChangesAsync();
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

