using CommunityToolkit.Mvvm.ComponentModel;
using ConvenienceStoreManagement.Components.ViewModel;
using System.Collections.ObjectModel;
using System.Linq;

namespace ConvenienceStoreManagement.Model.Control
{
    public partial class ListCartControl : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<CartItemViewModel> listCart = new();

        public void AddGood(GoodModel goodModel)
        {
            int index = ListCart.IndexOf(ListCart.Where(i => i.CompareId(goodModel)).FirstOrDefault());
            if (index >= 0)
            {
                ListCart[index].AddGood(goodModel);
            }
            else
            {
                CartItemViewModel cartItem = new(goodModel.Item);
                cartItem.AddGood(goodModel);
                ListCart.Add(cartItem);
            }
        }

        public bool RemoveGoodById(int id)
        {
            foreach (var element in ListCart)
            {
                if (element.RemoveGoodById(id))
                    return true;
            }

            return false;
        }

        public void ClearCart()
        {
            ListCart.Clear();
        }
    }
}
