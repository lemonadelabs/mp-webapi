using System;
using System.ComponentModel.DataAnnotations;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public class ProjectViewModel : ViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Reference { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        [Required]
        [EmailAddress]
        public MerlinPlanUser Creator { get; set; }

        [Required]
        public Group Group { get; set; }
    }
}
