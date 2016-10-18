using System.Reflection;
using System.Threading.Tasks;
using MPWebAPI.Models;

namespace MPWebAPI.ViewModels
{
    public class ViewModel
    {
        public virtual void MapToModel(object model)
        {
            MapProperties(this, model);
        }
        
        public virtual Task MapToViewModelAsync(object model, IMerlinPlanRepository repo = null)
        {
            MapProperties(model, this);
            return Task.CompletedTask;
        }
        
        private void MapProperties(object from, object to)
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