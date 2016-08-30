namespace MPWebAPI.Models
{
    public class MerlinPlanRepository : IMerlinPlanRepository
    {
        private readonly PostgresDBContext _dbcontext;
        
        public MerlinPlanRepository(PostgresDBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
    }    
}

