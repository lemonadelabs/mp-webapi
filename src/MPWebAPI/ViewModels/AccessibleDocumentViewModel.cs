using System.Collections.Generic;
using System.Linq;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public class AccessibleDocumentViewModel<T> where T : ViewModel, new()
    {
        public List<GroupViewModel> Groups { get; set; }
        public List<T> Documents { get; set; }
    }
}
