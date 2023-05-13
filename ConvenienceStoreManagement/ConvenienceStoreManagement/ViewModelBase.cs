using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using ConvenienceStoreManagement.Database;

namespace ConvenienceStoreManagement
{
    public class ViewModelBase : ObservableObject
    {
        protected Window? ViewWindow;
        protected DbManager? dbManager;

        public virtual ViewModelBase SetViewWindow(Window viewWindow)
        {
            ViewWindow = viewWindow;
            return this;
        }

        public virtual ViewModelBase SetDatabaseConnection(DbManager dbManager)
        {
            this.dbManager = dbManager;
            return this;
        }

        public virtual ViewModelBase FinishBuild()
        {
            return this;
        }
    }
}
