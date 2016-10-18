using System.ComponentModel.DataAnnotations;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public class GroupViewModel : ViewModel
    {
        public GroupViewModel(Group g)
        {
            MapToViewModelAsync(g);
        }

        public GroupViewModel() 
        {
            Active = true;
        }
        
        public int Id { get; set; }

        public bool Active { get; set; }

        [Required]
        public int OrganisationId { get; set; }
        
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}