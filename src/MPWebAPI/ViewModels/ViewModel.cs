using System.Reflection;

namespace MPWebAPI.ViewModels
{
    public class ViewModel
    {
        protected virtual void MapToModel(object model)
        {
            MapProperties(this, model);
        }
        
        protected void MapToViewModel(object model)
        {
            MapProperties(model, this);
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