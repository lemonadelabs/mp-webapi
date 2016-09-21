using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public class GroupViewModel : ViewModel
    {
        public GroupViewModel(Group g)
        {
            MapToViewModel(g);
        }

        public GroupViewModel() {}
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


    }
}