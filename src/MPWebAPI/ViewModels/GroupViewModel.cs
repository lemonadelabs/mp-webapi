using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public sealed class GroupViewModel : ViewModel
    {
        public GroupViewModel(Group g)
        {
            MapToViewModelAsync(g);
        }

        public GroupViewModel() 
        {
            Active = true;
        }

        public override Task<ViewModelMapResult> MapToViewModelAsync(object model, IMerlinPlanRepository repo = null)
        {
            base.MapToViewModelAsync(model, repo);
            var group = (Group) model;
            Portfolios = group.Portfolios?.Select(p => new SharedDocumentDetails
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();
            ResourceScenarios = group.ResourceScenarios?.Select(rs => new SharedDocumentDetails
            {
                Id = rs.Id,
                Name = rs.Name
            }).ToList();
            Projects = group.Projects?.Select(p => new SharedDocumentDetails
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();
            return Task.FromResult(new ViewModelMapResult());
        }

        public int Id { get; set; }

        public bool Active { get; set; }

        [Required]
        public int OrganisationId { get; set; }
        
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public List<SharedDocumentDetails> Portfolios { get; set; }
        public List<SharedDocumentDetails> ResourceScenarios { get; set; }
        public List<SharedDocumentDetails> Projects { get; set; }

        public class SharedDocumentDetails
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}