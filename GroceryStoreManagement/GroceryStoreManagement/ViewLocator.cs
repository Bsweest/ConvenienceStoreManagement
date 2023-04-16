using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System;

namespace GroceryStoreManagement
{
    public class ViewLocator : IDataTemplate
    {
        public IControl Build(object param)
        {
            var name = param.GetType().FullName!.Replace("ViewModel", "Page");
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type)!;
            }
            else
            {
                return new TextBlock { Text = "Not Found: " + name };
            }
        }

        public bool Match(object data)
        {
            return data is ViewModelBase;
        }
    }
}
