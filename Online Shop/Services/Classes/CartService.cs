using Online_Shop.Models;
using Online_Shop.Repository.Intefaces;
using Online_Shop.Services.Interfaces;
using Online_Shop.ViewModels;

namespace Online_Shop.Services.Classes
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;   
        }
        public void Add(CartViewModel cart)
        {
            _cartRepository.Add(MapToModel(cart));
        }

        public bool AddProductToCart(ProductViewModel product, int cartId)
        {
            return _cartRepository.AddProductToCart(product, cartId);
        }

        public void Delete(CartViewModel cart)
        {
            Cart oldCart = _cartRepository.GetById(cart.Id); 
            _cartRepository.Delete(oldCart);
        }

        public List<CartViewModel> GetAll()
        {
            List<Cart> carts = _cartRepository.GetAll();
            List<CartViewModel> cartViewModels = new List<CartViewModel>();
            foreach(Cart cart in carts)
            {
                cartViewModels.Add(MapToViewModel(cart));
            }
            return cartViewModels;
        }

        public CartViewModel GetById(int id)
        {
            return MapToViewModel(_cartRepository.GetById(id));
        }

        public List<CartViewModel> GetUserCarts(string userId)
        {
            List<Cart> userCarts = _cartRepository.GetUserCarts(userId);
            List<CartViewModel> cartViewModels = new List<CartViewModel>();
            foreach (Cart cart in userCarts)
            {
                cartViewModels.Add(MapToViewModel(cart));
            }
            return cartViewModels;
        }

        public CartViewModel GetUserInProgressCart(string userId)
        {
            return MapToViewModel(_cartRepository.GetUserInProgressCart(userId));
        }

        public Cart MapToModel(CartViewModel cartVM)
        {
            Cart cart = new Cart();
            cart.ApplicationUserId = cartVM.ApplicationUserId;
            cart.CartTotalPrice = cartVM.CartTotalPrice;
            cart.CartStatusId = cartVM.CartStatusId;

            return cart;
        }

        public CartViewModel MapToViewModel(Cart model)
        {
            CartViewModel cartViewModel = new CartViewModel();
            cartViewModel.Id = model.Id;
            cartViewModel.ApplicationUserId = model.ApplicationUserId;
            cartViewModel.CartTotalPrice = model.CartTotalPrice;
            cartViewModel.CartStatusId = model.CartStatusId;
            cartViewModel.PurchasingDate = model.PurchasingDate;

            return cartViewModel;
        }

        public void RemoveProductFromCart(int productCartId)
        {
            _cartRepository.RemoveProductFromCart(productCartId);
        }

        public void Update(CartViewModel cart)
        {
            Cart oldCart = _cartRepository.GetById(cart.Id);
            oldCart.ApplicationUserId = cart.ApplicationUserId;
            oldCart.CartTotalPrice = cart.CartTotalPrice;
            oldCart.CartStatusId = cart.CartStatusId;
            oldCart.PurchasingDate= cart.PurchasingDate;

            _cartRepository.Update(oldCart);
        }
    }
}
