using CommunityToolkit.Mvvm.ComponentModel;
using ConvenienceStoreManagement.Components.ViewModel.Base;

namespace ConvenienceStoreManagement.Components.ViewModel
{
    public partial class ChangeAuthViewModel : BaseBoxViewModel
    {
        private readonly int Id;

        [ObservableProperty]
        private string _username = string.Empty;
        [ObservableProperty]
        private string _check = string.Empty;
        [ObservableProperty]
        private string _password = string.Empty;

        public async override void OKBehaviour()
        {
            var isGrant = await dbManager.QueryEmployee
                                .GrantAdminControl(Check);
            if (!isGrant)
            {
                dbManager.Notify("the admin string is not matched", true);
                return;
            }

            var result = await dbManager.QueryEmployee
                .ChangeAuthEmployee(Id, Username, Password);

            if (result["data"] is object)
            {
                dbManager.Notify($"Change Auth of employee {Id} Successfull");
                CloseBehaviour();
            }
        }
    }
}
