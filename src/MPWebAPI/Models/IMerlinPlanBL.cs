using System.Threading.Tasks;

namespace MPWebAPI.Models
{
    /// <summary>
    /// Encapsulates the business logic for the Merlin: Plan application.
    /// </summary>
    public interface IMerlinPlanBL
    {
        Task CreateOrganisation(Organisation org);
    }    
}

