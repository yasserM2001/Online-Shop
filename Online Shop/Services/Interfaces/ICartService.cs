using Online_Shop.Models;
using Online_Shop.ViewModels;

namespace Online_Shop.Services.Interfaces
{
    public interface ICartService
    {
        CartViewModel GetById(int id);
        List<CartViewModel> GetAll();
        List<CartViewModel> GetUserCarts(string userId);
        CartViewModel GetUserInProgressCart(string userId);
        void Add(CartViewModel cart);
        void Update(CartViewModel cart);
        void Delete(CartViewModel cart);
        CartViewModel MapToViewModel(Cart model);
        Cart MapToModel(CartViewModel cartVM);
        bool AddProductToCart(ProductViewModel product,int cartId);
        public void RemoveProductFromCart(int productCartId);
    }
}
