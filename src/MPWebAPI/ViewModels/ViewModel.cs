using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{


    public class ViewModelMapResult
    {
        public ViewModelMapResult()
        {
            Succeeded = true;
            Errors = new Dictionary<string, List<string>>();
        }

        public void AddError(string key, string error)
        {
            Succeeded = false;
            if (!Errors.ContainsKey(key))
            {
                Errors.Add(key, new List<string>());
            }
            Errors[key].Add(error);
        }
        public bool Succeeded { get; set; }

        public Dictionary<string, List<string>> Errors { get; set; }
    }


    public class ViewModel
    {
        public virtual Task<ViewModelMapResult> MapToModel(object model, IMerlinPlanRepository repo = null)
        {
            MapProperties(this, model);
            return Task.FromResult(new ViewModelMapResult());
        }
        
        public virtual Task<ViewModelMapResult> MapToViewModelAsync(object model, IMerlinPlanRepository repo = null)
        {
            MapProperties(model, this);
            return Task.FromResult(new ViewModelMapResult());
        }
        
        private static void MapProperties(object from, object to)
        {
            foreach (var toPropInfo in to.GetType().GetProperties())
            {
                foreach (var fromPropInfo in from.GetType().GetProperties())
                {
                    if (
                        toPropInfo.PropertyType == fromPropInfo.PropertyType &&
                        toPropInfo.Name == fromPropInfo.Name 
                        )
                    {
                        toPropInfo.SetValue(to, fromPropInfo.GetValue(from));
                    }                    
                }
            }
        }
    }
}