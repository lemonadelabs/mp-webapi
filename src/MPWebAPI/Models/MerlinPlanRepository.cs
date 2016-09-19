using System;
using System.Collections.Generic;
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

        public void AddOrganisation(Organisation org)
        {
            throw new NotImplementedException();
        }

        public void AddOrganisations(IEnumerable<Organisation> orgs)
        {
            throw new NotImplementedException();
        }

        public void RemoveOrganisation(int orgId)
        {
            throw new NotImplementedException();
        }

        public void RemoveOrganisations(IEnumerable<int> orgIds)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrganisation(Organisation org)
        {
            throw new NotImplementedException();
        }
    }    
}

