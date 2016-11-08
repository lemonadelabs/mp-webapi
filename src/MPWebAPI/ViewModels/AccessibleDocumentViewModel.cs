using System.Collections.Generic;
using System.Linq;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public class AccessibleDocumentViewModel<T> where T : ViewModel, new()
    {
        public class UserShared
        {
            public UserViewModel User { get; set; }
            public List<T> Documents { get; set; }
        }

        public class GroupShared
        {
            public GroupViewModel Group { get; set; }
            public List<T> Documents { get; set; }
        }

        public List<T> Created { get; set; }
        public List<GroupShared> GroupShare { get; set; }
        public List<UserShared> UserShare { get; set; }
        public List<GroupShared> OrgShare { get; set; }

        public static List<GroupShared> DocumentsByGroup(IEnumerable<IMerlinPlanDocument> documents)
        {
            var groupDocuments = new List<GroupShared>();
            foreach (var vm in documents)
            {
                var g = groupDocuments.FirstOrDefault(ass => ass.Group.Id == vm.Group.Id);
                if (g != null)
                {
                    var newT = new T();
                    newT.MapToViewModelAsync(vm);
                    g.Documents.Add(newT);
                }
                else
                {
                    var newT = new T();
                    newT.MapToViewModelAsync(vm);
                    var newass = new GroupShared
                    {
                        Group = new GroupViewModel(vm.Group),
                        Documents = new List<T>(
                            new[] { newT })
                    };
                    groupDocuments.Add(newass);
                }
            }
            return groupDocuments;
        }
    }
}
