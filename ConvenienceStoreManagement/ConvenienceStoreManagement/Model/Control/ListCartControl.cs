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

        [ObservableProperty]
        public int totalCost;

        public void CalculateTotal()
        {
            var x = 0;
            foreach (var item in ListCart)
            {
                x += item.TotalCost;
            }
            TotalCost = x;
        }

        public void AddGood(GoodModel goodModel)
        {
            int index = ListCart.IndexOf(ListCart.FirstOrDefault(i => i.CompareId(goodModel)));
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

            CalculateTotal();
        }

        public bool RemoveGoodById(int id)
        {
            foreach (var element in ListCart)
            {
                if (element.RemoveGoodById(id))
                {
                    if (element.Goods.Count == 0) ListCart.Remove(element);
                    return true;
                }
            }

            CalculateTotal();
            return false;
        }

        public bool CheckExistedInCart(int id)
        {
            foreach (var element in ListCart)
            {
                var check = element.Goods.FirstOrDefault(i => i.Id == id);
                if (check != null)
                {
                    return true;
                }
            }
            return false;
        }

        public void ClearCart()
        {
            ListCart.Clear();
            CalculateTotal();
        }
    }
}
