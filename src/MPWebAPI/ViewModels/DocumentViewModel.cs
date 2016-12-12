using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public class DocumentViewModel<T, TU> : ViewModel where T : IMerlinPlanDocument<TU> where TU : IDocumentUser
    {
        [Required]
        public int Group { get; set; }

        [Required]
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        [Required]
        [EmailAddress]
        public string Creator { get; set; }
        public UserDetails CreatorDetails { get; set; }
        public ShareDetails Sharing { get; set; }

        public virtual Task<ViewModelMapResult> MapToViewModelAsync(T model, IMerlinPlanRepository repo = null)
        {
            base.MapToViewModelAsync(model, repo);

            Creator = model.Creator.Id;
            Group = model.Group.Id;
            CreatorDetails = new UserDetails
            {
                FirstName = model.Creator.FirstName,
                LastName = model.Creator.LastName
            };

            Sharing = new ShareDetails
            {
                GroupShared = model.ShareGroup,
                OrganisationShared = model.ShareAll,
                UserShare = model.ShareUser?.Select(su => su.UserId).ToArray() ?? new string[] {}
            };

            return new Task<ViewModelMapResult>(() => new ViewModelMapResult());
        }

        public virtual async Task<ViewModelMapResult> MapToModel(T model, IMerlinPlanRepository repo = null)
        {
            if(repo == null) throw new ArgumentNullException(nameof(repo));
            var result = await base.MapToModel(model, repo);
            model.Creator = repo.Users.SingleOrDefault(u => u.Id == Creator);
            model.Group = repo.Groups.SingleOrDefault(g => g.Id == Group);
            if (model.Group != null) return result;
            result.AddError("Group", $"The Group with id {Group} can't be found");
            return result;
        }

        public class UserDetails
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class ShareDetails
        {
            public bool GroupShared { get; set; }
            public bool OrganisationShared { get; set; }
            public string[] UserShare { get; set; }
        }
    }
}