using Online_Shop.Models;
using Online_Shop.ViewModels;

namespace Online_Shop.Repository.Intefaces
{
    public interface ICartRepository
    {
        Cart GetById(int id);
        List<Cart> GetAll();
        List<Cart> GetUserCarts(string userId);
        Cart GetUserInProgressCart(string userId);
        void Add(Cart cart);
        void Update(Cart cart);
        void Delete(Cart cart);
        bool AddProductToCart(ProductViewModel product, int cartId);
        public void RemoveProductFromCart(int productCartId);

    }
}
